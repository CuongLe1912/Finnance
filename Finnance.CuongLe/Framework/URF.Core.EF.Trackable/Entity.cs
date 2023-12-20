using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using TrackableEntities.Common.Core;
using System.Text.Json.Serialization;
using System.Runtime.Serialization;

namespace URF.Core.EF.Trackable
{
    public abstract class Entity : ITrackable, IMergeable
    {
        [NotMapped]
        [JsonIgnore]
        [IgnoreDataMember]
        public Guid EntityIdentifier { get; set; }

        [NotMapped]
        [JsonIgnore]
        [IgnoreDataMember]
        public TrackingState TrackingState { get; set; }

        [NotMapped]
        [JsonIgnore]
        [IgnoreDataMember]
        public ICollection<string> ModifiedProperties { get; set; }
    }
}   