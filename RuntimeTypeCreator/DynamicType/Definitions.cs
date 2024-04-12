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

        public readonly int SizeLimit;

        public FieldDefinition(string fieldName, string typeName)
        {
            FieldName = fieldName;
            TypeName = typeName;
        }

        public FieldDefinition(string fieldName, string typeName, int sizeLimit)
            : this(fieldName, typeName)
        {
            SizeLimit = sizeLimit;
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
            sb.AppendLine("using RuntimeTypeCreator.DynamicType.Attributes;");
            sb.AppendLine();
            sb.AppendLine("namespace RuntimeTypeCreator.DynamicType.Generated {");
            sb.AppendLine();
            sb.AppendLine("[Structure]");
            sb.AppendLine($"\tpublic struct {StructName} {{");
            foreach (var field in Fields)
            {
                if (field.SizeLimit > 0)
                    sb.AppendLine($"\t\t[Size({field.SizeLimit})]");
                sb.AppendLine($"\t\tpublic {field.TypeName} {field.FieldName};");
            }
            sb.AppendLine("\t}");
            sb.AppendLine("}");

            return sb.ToString();
        }
    }
}
