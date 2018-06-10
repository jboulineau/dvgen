namespace dvgen.Model
{
    public class Script
    {
        public string Body { get; set; }
        public string Name { get; set; }
        public ScriptType Type { get; set; }
    }

    public enum ScriptType
    {
        api_udt,
        hub_udt,
        link_udt,
        satelite_udt,
        hub_table,
        link_table,
        satellite_table,
        hub_insert,
        link_insert,
        satellite_insert,
        hub_delete,
        link_delete,
        satellite_delete
    }
}