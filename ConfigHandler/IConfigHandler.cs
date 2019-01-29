namespace Mirle.A33.ConfigHandler
{
    public interface IConfigHandler
    {
        string FilePath { get; set; }
        string KeyName { get; set; }
        string SectionName { get; set; }

        bool GetDataToBoolean(string SectionName, string KeyName, string DefaultValue);
        string GetDataToString(string SectionName, string KeyName, string DefaultValue);
        void SetDataFromBool(string SectionName, string KeyName, bool SetValueInBool);
        void SetDataFromString(string SectionName, string KeyName, string SetValue);
    }
}