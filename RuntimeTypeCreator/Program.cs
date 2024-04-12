using RuntimeTypeCreator.DynamicType;

namespace RuntimeTypeCreator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var stInfo = new StructDefinition("ST_Info");
            stInfo.Fields.AddRange([new FieldDefinition("Version", "string", 25), new FieldDefinition("StatusCode", "int")]);

            var stButton = new StructDefinition("ST_Button");
            stButton.Fields.AddRange([new FieldDefinition("Pressed", "bool"), new FieldDefinition("Active", "bool"), new FieldDefinition("Enabled", "bool")]);

            var stMachine = new StructDefinition("ST_Machine");
            stMachine.Fields.AddRange([new FieldDefinition("Buttons", "ST_Button[]", 10), new FieldDefinition("Info", "ST_Info")]);

            var generator = new TypeGenerator();
            var typeMap = generator.Generate([stInfo, stButton, stMachine]);
        }
    }
}
