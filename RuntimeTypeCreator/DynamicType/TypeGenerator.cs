using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.CodeDom.Compiler;
using System.Reflection;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System.Runtime.Loader;

namespace RuntimeTypeCreator.DynamicType
{
    internal class TypeGenerator
    {
        public Dictionary<string, Type> Generate(List<StructDefinition> structs)
        {
            var typeMap = new Dictionary<string, Type>();

            List<string> code = new List<string>();
            foreach(var entry in structs)
            {
                var gen = entry.GenerateCode();
                code.Add(gen);

                File.WriteAllText($"{entry.StructName}.g.cs", gen);
            }

            var syntaxTrees = new List<SyntaxTree>();
            foreach (var implementation in code)
                syntaxTrees.Add(CSharpSyntaxTree.ParseText(implementation));

            var refPaths = new[] {
                typeof(System.Object).GetTypeInfo().Assembly.Location,
                Path.Combine(Path.GetDirectoryName(typeof(System.Runtime.GCSettings).GetTypeInfo().Assembly.Location), "System.Runtime.dll")
            };
            MetadataReference[] references = refPaths.Select(r => MetadataReference.CreateFromFile(r)).ToArray();

            var compilation = CSharpCompilation.Create(
                        "RuntimeTypeCreator.Generated.dll",
                        syntaxTrees: syntaxTrees,
                        references: references,
                        options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            using var stream = new MemoryStream();
            var result = compilation.Emit(stream);

            if (result.Success)
            {
                stream.Seek(0, SeekOrigin.Begin);
                var generatedAssembly = AssemblyLoadContext.Default.LoadFromStream(stream);

                foreach (var entry in structs)
                {
                    var generatedType = generatedAssembly.GetType($"RuntimeTypeCreator.DynamicType.Generated.{entry.StructName}");
                    typeMap.Add(entry.StructName, generatedType);

                    var instance = Activator.CreateInstance(generatedType);
                }
            }
            else
            {
                IEnumerable<Diagnostic> failures = result.Diagnostics.Where(diagnostic =>
                    diagnostic.IsWarningAsError ||
                    diagnostic.Severity == DiagnosticSeverity.Error);

                string log = "compiler_error.txt";
                if (File.Exists(log))
                    File.Delete(log);

                var builder = new StringBuilder();
                foreach (Diagnostic diagnostic in failures)
                {
                    builder.AppendLine($"{diagnostic.Id}: {diagnostic.GetMessage()}");
                }
                File.WriteAllText(log, builder.ToString());
            }

            return typeMap;
        }
    }
}
