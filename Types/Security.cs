﻿using System;
using Newtonsoft.Json;

namespace Roblox.Reflection
{
    public enum SecurityType
    {
        None,
        PluginSecurity,
        LocalUserSecurity,
        RobloxScriptSecurity,
        RobloxSecurity,
        NotAccessibleSecurity
    }

    public class Security
    {
        public SecurityType Type;
        public string Prefix;

        public Security(string security, string prefix = "")
        {
            Enum.TryParse(security, out Type);
            Prefix = prefix;
        }

        public static implicit operator Security(string security)
        {
            return new Security(security);
        }

        public string Describe(bool displayNone)
        {
            string result = "";

            if (displayNone || Type != SecurityType.None)
                result += '{' + Prefix + Program.GetEnumName(Type) + '}';

            return result;
        }

        public override string ToString()
        {
            return Describe(false);
        }
    }

    public class ReadWriteSecurity
    {
        public Security Read;
        public Security Write;

        public bool Merged => (Read.Type == Write.Type);

        [JsonConstructor]
        public ReadWriteSecurity(string read, string write)
        {
            Read = new Security(read);
            Write = new Security(write, "✏️");
        }

        public string Describe(bool displayNone)
        {
            string result = "";

            string read = Read.Describe(displayNone);
            string write = Write.Describe(displayNone);

            if (read.Length > 0)
                result += read;

            if (Read.Type != Write.Type && write.Length > 0)
                result += ' ' + write;

            return result.Trim();
        }

        public override string ToString()
        {
            return Describe(false);
        }
    }
}