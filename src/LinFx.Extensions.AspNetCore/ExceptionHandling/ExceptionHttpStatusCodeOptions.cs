﻿using System.Net;

namespace LinFx.Extensions.AspNetCore.ExceptionHandling;

public class ExceptionHttpStatusCodeOptions
{
    public IDictionary<string, HttpStatusCode> ErrorCodeToHttpStatusCodeMappings { get; } = new Dictionary<string, HttpStatusCode>();

    public void Map(string errorCode, HttpStatusCode httpStatusCode) => ErrorCodeToHttpStatusCodeMappings[errorCode] = httpStatusCode;
}
