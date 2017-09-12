using System;
using System.Collections.Generic;
using System.Text;
using Mono.Cecil;

namespace ILSmasher
{
    //public class GenericParameterGenerator
    //{
    //    private GenericParameter _oldParam;
    //    private GenericParameter _newParam;
    //    private IGenericParameterProvider _provider;

    //    public GenericParameterGenerator(GenericParameter oldParm, IGenericParameterProvider provider)
    //    {
    //        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
    //        _oldParam = oldParm;
    //        
    //    }

    //    public GenericParameter Defintion => _newParam;

    //    public void Finish()
    //    {
    //        TypeMapper.GetTypeReferences(_oldParam.Constraints, _newParam.Constraints, _newParam);
    //        //// delay copy to ensure all generics parameters are already present
    //        //Copy(input, output, (gp, ngp) => CopyTypeReferences(gp.Constraints, ngp.Constraints, nt));
    //        //Copy(input, output, (gp, ngp) => CopyCustomAttributes(gp.CustomAttributes, ngp.CustomAttributes, nt));

    //    }
    //}
}
