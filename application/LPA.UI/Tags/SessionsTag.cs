namespace LPA.UI.Tags
{
    public class SessionsTag
        : IStandardTag
    {
        private static readonly SessionsTag instance = new SessionsTag();

        private SessionsTag()
        {
        }

        public static SessionsTag Instance
        {
            get
            {
                return instance;
            }
        }

        public string Type => "Sessions";
    }
}
