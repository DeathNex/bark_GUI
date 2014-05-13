namespace bark_GUI.CustomControls
{
    public interface ICustomControl
    {
        string Name { get; set; }

        string Help { get; set; }

        bool IsRequired { get; set; }

        string Value { get; set; }

        string Unit { get; set; }

        string UnitX { get; set; }
    }
}
