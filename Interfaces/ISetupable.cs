namespace VogueVR.Heartbeat
{
    /// <summary>
    /// Reference: https://github.com/vmuijrers/GitGud/blob/main/Assets/Scripts/ExampleProject/IUpdate.cs
    /// </summary>
    public interface ISetupable 
    {
        int SetupOrder { get; }
        void DoSetup();
    }
}