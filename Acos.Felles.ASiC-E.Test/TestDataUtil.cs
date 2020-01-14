using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Acos.Felles.ASiCE.Test
{
    public static class TestDataUtil
    {

        public static Stream ReadValidAsiceCadesFromResource()
        {
            return ReadResource("cades-valid.asice");
        }

        public static Stream ReadResource(string resource)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = assembly.GetManifestResourceNames().Single(str =>
                str.EndsWith(resource, StringComparison.CurrentCulture));
            return assembly.GetManifestResourceStream(resourceName);
        }
    }
}