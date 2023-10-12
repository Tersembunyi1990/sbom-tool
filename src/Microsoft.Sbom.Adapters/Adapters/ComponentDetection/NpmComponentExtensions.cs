// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Sbom.Adapters.ComponentDetection;

using System;
using Microsoft.ComponentDetection.Contracts.Internal;
using Microsoft.ComponentDetection.Contracts.TypedComponent;
using Microsoft.Sbom.Contracts;

/// <summary>
/// Extensions methods for <see cref="NpmComponent" />.
/// </summary>
internal static class NpmComponentExtensions
{
    /// <summary>
    /// Converts a <see cref="NpmComponent" /> to an <see cref="SbomPackage" />.
    /// </summary>
    /// <param name="npmComponent">The <see cref="NpmComponent" /> to convert.</param>
    /// <param name="license"> License information for the package that component that is being converted.</param>
    /// <returns>The converted <see cref="SbomPackage" />.</returns>
    public static SbomPackage ToSbomPackage(this NpmComponent npmComponent, string? license = null) => new()
    {
        Id = npmComponent.Id,
        PackageUrl = npmComponent.PackageUrl?.ToString(),
        PackageName = npmComponent.Name,
        PackageVersion = npmComponent.Version,
        Checksum = new[]
        {
            new Checksum
            {
                ChecksumValue = npmComponent.Hash,
            },
        },
        Supplier = npmComponent.Author?.AsSupplier(),
        LicenseInfo = string.IsNullOrWhiteSpace(license) ? null : new LicenseInfo
        {
            Concluded = license,
        },
        FilesAnalyzed = false,
        Type = "npm",
    };

    /// <summary>
    /// Converts the <see cref="NpmAuthor" /> to an SPDX Supplier.
    /// </summary>
    /// <param name="npmAuthor">The <see cref="NpmAuthor" /> to convert.</param>
    /// <returns>The SPDX Supplier.</returns>
    private static string AsSupplier(this NpmAuthor npmAuthor) => (npmAuthor.Name, npmAuthor.Email) switch
    {
        ({ } name, { } email) => $"Organization: {name} ({email})",
        ({ } name, _) => $"Organization: {name}",
        _ => throw new InvalidOperationException("NpmAuthor must have a name."),
    };
}
