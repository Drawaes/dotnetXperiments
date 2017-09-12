using System;
using System.Collections.Generic;
using System.Text;
using Mono.Cecil;

namespace ILSmasher
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
                if (_scope != newToken._scope)
                {
                    return false;
                }
                if(_token != newToken._token)
                {
                    return false; 
                }
                return true;
            }
            return false;
        }

        public override int GetHashCode() => _token.GetHashCode() ^ _scope.GetHashCode();

        public override string ToString() => $"{_scope} Type - {_token.TokenType} RID - {_token.RID}";
    }
}
