using Mono.Cecil;

namespace ILSmasher
{
    public class MethodGenerator
    {
        private MethodDefinition _methodDefinition;
        private string _name;

        public MethodGenerator(MethodDefinition methodDefintion)
        {
            _methodDefinition = methodDefintion;
            _name = methodDefintion.SafeName();
        }

        public string Name => _name;
    }
}