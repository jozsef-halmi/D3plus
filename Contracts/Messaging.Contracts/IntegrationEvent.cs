﻿using System.Text.Json.Serialization;

namespace Messaging.Contracts;

public class IntegrationEvent
{
    public IntegrationEvent()
    {
        Id = Guid.NewGuid();
        CreationDate = DateTime.UtcNow;
    }

    [JsonConstructor]
    public IntegrationEvent(Guid id, DateTime createDate)
    {
        Id = id;
        CreationDate = createDate;
    }

    [JsonInclude]
    public Guid Id { get; private init; }

    [JsonInclude]
    public DateTime CreationDate { get; private init; }

    [JsonInclude]
    public string TraceRootId { get; set; }

    [JsonInclude]
    public string TraceParentId { get; set; }
}