﻿using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Sbom.Config;
using Microsoft.Sbom.Entities;
using Microsoft.Sbom.Enums;
using Microsoft.Sbom.Interfaces;

namespace Microsoft.Sbom.Creation;
public class RunAsUserInfoProvider : ISourceProvider
{
    private readonly ILogger logger;
    private readonly Configuration? configuration;

    public RunAsUserInfoProvider(Configuration? configuration, ILogger? logger)
    {
        this.logger = logger ?? NullLogger.Instance;
        this.configuration = configuration;
    }

    public SourceType SourceType => SourceType.UserInfo;

    public async IAsyncEnumerable<object> Get()
    {
        this.logger.LogDebug("Got run as user name: {name}", Environment.UserName);
        yield return await Task.FromResult(new UserInfo(Environment.UserName, string.Empty));
    }
}
