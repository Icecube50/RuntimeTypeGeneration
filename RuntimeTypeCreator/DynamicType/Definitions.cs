using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuntimeTypeCreator.DynamicType
{
    internal class FieldDefinition
    {
        public readonly string FieldName;

        public readonly string TypeName;

        public FieldDefinition(string fieldName, string typeName)
        {
            FieldName = fieldName;
            TypeName = typeName;
        }
    }

    internal class StructDefinition
    {
        public readonly string StructName;

        public readonly List<FieldDefinition> Fields;

        public StructDefinition(string structName)
        {
            StructName = structName;

            Fields = new List<FieldDefinition>();
        }

        public string GenerateCode()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("using System;");
            sb.AppendLine();
            sb.AppendLine("namespace RuntimeTypeCreator.DynamicType.Generated {");
            sb.AppendLine($"\tpublic struct {StructName} {{");
            foreach (var field in Fields)
            {
                sb.AppendLine($"\t\tpublic {field.TypeName} {field.FieldName};");
            }
            sb.AppendLine("\t}");
            sb.AppendLine("}");

            return sb.ToString();
        }
    }
}
