﻿namespace PlayCommon
{
    public interface IEntity
    {
        Guid Id { get; set; }
        DateTimeOffset CreatedDate { get; set; }
    }
}
