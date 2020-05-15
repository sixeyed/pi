using System;

namespace Pi.Api.Model
{
    public class Job
    {
        public int DecimalPlaces { get; set; }

        public string Id { get; private set; }

        public string ProcessingId { get; set; }

        public Job()
        {
            Id = Guid.NewGuid().ToString().Substring(0, 6);
        }
    }
}
