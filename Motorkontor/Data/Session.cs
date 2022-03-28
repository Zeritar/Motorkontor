using System;

namespace Motorkontor.Data
{
    public class Session
    {
        public string gUID { get; private set; }
        public virtual Login login { get; set; }
        
        public DateTime startTime { get; set; }

        public bool hasChanged { get; set; } = false;

        public bool hasExpired()
        {
            return (uint)(DateTime.Now - startTime).TotalSeconds >= 900;
        }

        public Session() : this(Guid.NewGuid().ToString())
        {
        }

        public Session(string guid)
        {
            startTime = DateTime.UnixEpoch;
            gUID = guid;
        }
    }
}
