﻿using System.Collections.Generic;

namespace Roblox.Reflection
{
    public sealed class PropertyDescriptor : MemberDescriptor
    {
        public string Category;
        public LuaType ValueType;
        public ReadWriteSecurity Security;
        public Serialization Serialization;

        public override string GetSchema(bool detailed = true)
        {
            string schema = base.GetSchema();

            if (detailed)
                schema = "{ValueType} " + schema + " {Security} {Serialization} {Tags} {ThreadSafety}";

            return schema;
        }

        public override Dictionary<string, object> GetTokens(bool detailed = false)
        {
            var tokens = base.GetTokens(detailed);
            
            if (detailed)
                return tokens;

            // Only provide a serialization token if the save/load states differ.
            if (Serialization.CanSave == Serialization.CanLoad)
                tokens.Remove("Serialization");
            else
                tokens["Serialization"] = Serialization.Describe(false);

            return tokens;
        }

        public override int CompareTo(object other)
        {
            if (other is PropertyDescriptor)
            {
                var otherDesc = other as PropertyDescriptor;

                if (Class != otherDesc.Class)
                    return Class.CompareTo(otherDesc.Class);

                bool thisIsCamel = char.IsLower(Name[0]);
                bool otherIsCamel = char.IsLower(otherDesc.Name[0]);

                // Upcast the comparison if this is a camelCase condition.
                // camelCase members should always appear last in the member type listing.
                if (thisIsCamel != otherIsCamel)
                    return base.CompareTo(other);

                // Compare by categories.
                if (Category != otherDesc.Category)
                {
                    return Category.CompareTo(otherDesc.Category);
                }
            }

            return base.CompareTo(other);
        }
    }
}