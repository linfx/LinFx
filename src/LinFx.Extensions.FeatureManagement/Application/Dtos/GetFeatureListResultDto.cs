﻿using Volo.Abp.FeatureManagement;

namespace LinFx.Extensions.FeatureManagement.Application.Dtos;

public class GetFeatureListResultDto
{
    public required List<FeatureGroupDto> Groups { get; set; }
}
