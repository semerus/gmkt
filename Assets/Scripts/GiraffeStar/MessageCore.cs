namespace GiraffeStar
{
    public abstract class MessageCore
    {
        public string service = "Default";

        public void Dispatch()
        {
            GiraffeSystem.ProcessMessage(this);
        }
    }
}


