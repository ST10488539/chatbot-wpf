using System;
using System.Collections.Generic;

namespace CyberSecurityChatbot
{
    class ResponseHandler
    {
        private Dictionary<string, List<string>> responses;
        private Random random = new Random();
        private string lastTopic = "";  
        private Dictionary<string, string> memory = new Dictionary<string, string>();

        public ResponseHandler()
        {
            responses = new Dictionary<string, List<string>>();

            responses["password"] = new List<string>
            {
                "🔑 **Password Security Breakdown:**\n\n" +
                "1. **Complexity:** Use a mix of upper/lowercase letters, numbers, and symbols (@, #, $).\n" +
                "2. **Length:** Make passwords at least 12-16 characters long. Length beats complexity!\n" +
                "3. **Management:** Never reuse the same password. Use a trusted Password Manager (like Bitwarden or 1Password) so you only have to remember one master password."
            };

            responses["phishing"] = new List<string>
            {
                "🎣 **Phishing Protection Guide:**\n\n" +
                "1. **Verify Sender Details:** Scammers masquerade as trusted brands, but check the actual email address behind the display name (e.g., security@paypaI-secure-update.com instead of paypal.com).\n" +
                "2. **Look for Urgency:** If an email screams 'Your account will be suspended in 24 hours!', it's likely a trap to make you panic and click.\n" +
                "3. **Inspect Links:** Hover your mouse over links *before* clicking them to see where they actually lead. When in doubt, log in directly through the official browser bookmark."
            };

            responses["privacy"] = new List<string>
            {
                "🔒 **Digital Privacy Essentials:**\n\n" +
                "1. **Two-Factor Authentication (2FA):** Always turn on 2FA using an authenticator app. Even if someone steals your password, they still can't access your account.\n" +
                "2. **Social Oversharing:** Avoid posting your location, full birthday, or pet names online—scammers scrape this info to guess security questions.\n" +
                "3. **App Permissions:** Audit your phone settings. A simple calculator or wallpaper app does not need access to your contact list or microphone."
            };

            responses["scam"] = new List<string>
            {
                "⚠️ **How to Spot and Avoid Scams:**\n\n" +
                "1. **The Financial Red Flag:** If an unknown source asks you to pay via Crypto, Gift Cards, or Wire Transfers, it's 100% a scam. These payment methods cannot be reversed.\n" +
                "2. **Impersonation:** Scammers pretend to be tech support (Microsoft/Apple), the bank, or even government officials. Hang up and call the official number published on their website.\n" +
                "3. **Too Good to be True:** Fake giveaways, crypto investment schemes promising guaranteed high returns, or sudden inheritance notifications are always traps."
            };

            responses["browsing"] = new List<string>
            {
                "🌐 **Safe Web Browsing Tactics:**\n\n" +
                "1. **Encryption Standards:** Look for the lock icon in your URL bar and ensure the address starts with **HTTPS** (the 'S' stands for Secure).\n" +
                "2. **Public Wi-Fi Danger:** Never log into banking apps or shop online while connected to free public Wi-Fi (like at a coffee shop or airport) unless you are using a trusted VPN.\n" +
                "3. **Software Hygiene:** Keep your web browser (Chrome, Edge, Firefox) completely updated. Most browser updates fix critical security vulnerabilities that hackers exploit."
            };
        }

        public void SetMemory(string key, string value)
        {
            key = key.ToLower();
            if (memory.ContainsKey(key))
                memory[key] = value;
            else
                memory.Add(key, value);
        }

        public string GetMemory(string key)
        {
            key = key.ToLower();
            if (memory.ContainsKey(key))
                return memory[key];
            return "";
        }

        public string GetResponse(string input)
        {
            input = input.ToLower().Trim();

            string name = GetMemory("name");
            string favTopic = GetMemory("favouriteTopic");
            string userNameText = !string.IsNullOrEmpty(name) ? ", " + name : "";

            if (input == "hi" || input == "hello" || input == "hey" || input.Contains("good morning") || input.Contains("good afternoon"))
            {
                if (!string.IsNullOrEmpty(name))
                    return $"Hey {name}! Welcome back. What cybersecurity topic are we checking out today?";
                return "Hello! I'm your Cyber Security assistant. What is your name?";
            }

            if (input == "bye" || input == "goodbye" || input == "see ya" || input == "thanks" || input == "thank you")
            {
                return $"You're very welcome{userNameText}! Stay safe out there. Chat soon! 🛡️";
            }

            if (string.IsNullOrEmpty(name))
            {
                string extractedName = input;
                if (extractedName.StartsWith("my name is "))
                    extractedName = extractedName.Replace("my name is ", "");
                else if (extractedName.StartsWith("i am "))
                    extractedName = extractedName.Replace("i am ", "");

                extractedName = extractedName.Trim();

                if (extractedName.Length > 0)
                {
                    extractedName = char.ToUpper(extractedName[0]) + extractedName.Substring(1);
                    SetMemory("name", extractedName);
                    return $"Nice to meet you, {extractedName}! Ask me anything about passwords, phishing, privacy, scams, or browsing.";
                }
            }
            else if (input.StartsWith("my name is ") || input.StartsWith("i am "))
            {
                string extractedName = input.Replace("my name is ", "").Replace("i am ", "").Trim();
                if (extractedName.Length > 0)
                {
                    extractedName = char.ToUpper(extractedName[0]) + extractedName.Substring(1);
                    SetMemory("name", extractedName);
                    return $"Got it! I'll call you {extractedName} from now on.";
                }
            }

            if (input.Contains("what is my name") || input.Contains("what's my name"))
            {
                if (!string.IsNullOrEmpty(name))
                    return $"Your name is {name}! I don't forget my friends.";
                return "I don't know your name yet! What should I call you?";
            }

            if (input.Contains("worried") || input.Contains("scared") || input.Contains("hacked"))
            {
                return $"It's completely understandable to feel anxious about security{userNameText}. Let's look at a comprehensive tip to protect yourself:\n\n" + GetRandomResponse("scam");
            }

            if (input.Contains("curious") || input.Contains("interested"))
            {
                return "That's great curiosity! Staying curious is the best way to stay safe online.\n\n" + GetRandomResponse("privacy");
            }

            if (input.Contains("frustrated") || input.Contains("confused"))
            {
                return $"Take a deep breath{userNameText}, cybersecurity takes time to learn. Step by step!";
            }

            if (input.Contains("tell me more") || input.Contains("another tip") || input == "more" || input == "next")
            {
                if (!string.IsNullOrEmpty(lastTopic))
                    return $"Here is the deep dive for **{lastTopic}**:\n\n" + GetRandomResponse(lastTopic);
                return "What would you like to hear more about? I know about passwords, phishing, privacy, scams, and browsing.";
            }

            foreach (var keyword in responses.Keys)
            {
                if (input.Contains(keyword))
                {
                    lastTopic = keyword;
                    string response = GetRandomResponse(keyword);

                    if (!string.IsNullOrEmpty(favTopic) && keyword == favTopic)
                    {
                        response += $"\n\n🌟 As someone interested in {favTopic}, this is crucial for you!";
                    }
                    return response;
                }
            }

            if (!string.IsNullOrEmpty(name))
            {
                return $"Hmm, I didn't quite catch that, {name}. Try asking SideKick something like 'how do I stop phishing?' or 'tell me about passwords'.";
            }

            return "I didn't quite understand that. Try asking SideKick about passwords, phishing, privacy, scams, or browsing.";
        }

        private string GetRandomResponse(string topic)
        {
            List<string> list = responses[topic];
            return list[random.Next(list.Count)];
        }
    }
}
