using System;
using System.Collections.Generic;
using System.Text;
using Mono.Cecil;

namespace Crusher2
{
    public struct Key
    {
        private readonly MetadataToken _token;
        private readonly string _scope;

        public Key(MetadataToken token, IMetadataScope scope)
        {
            _token = token;
            _scope = GetScopeName(scope);
        }

        public MetadataToken Token => _token;
        public string Scope => _scope;

        public static string GetScopeName(IMetadataScope scope)
        {
            switch (scope)
            {
                case AssemblyNameReference nr:
                    return nr.Name;
                case ModuleDefinition md:
                    return md.Assembly.Name.Name;
                default:
                    throw new InvalidOperationException($"Couldn't make a scope {scope}");
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is Key newToken)
            {
                return newToken == this;
            }
            return false;
        }

        public override int GetHashCode() => _token.GetHashCode() ^ _scope.GetHashCode();

        public override string ToString() => $"{_scope} Type - {_token.TokenType} RID - {_token.RID}";

        public static bool operator ==(Key c1, Key c2)
        {
            if (c1.Scope != c2.Scope)
            {
                return false;
            }
            if (c1.Token != c2.Token)
            {
                return false;
            }
            return true;
        }

        public static bool operator !=(Key c1, Key c2) => !(c1 == c2);
        public static bool operator !=(Key c1, TypeDefinition typeDef) => !(c1 == typeDef);

        public static bool operator ==(Key c1, TypeDefinition typeDef)
        {
            if (typeDef.MetadataToken != c1.Token)
            {
                return false;
            }
            if (GetScopeName(typeDef.Scope) != c1.Scope)
            {
                return false;
            }
            return true;
        }
    }
}
