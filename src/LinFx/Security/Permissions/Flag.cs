using System;

namespace LinFx.Security.Permissions
{
	/// <summary>
	/// 权限标记
	/// </summary>
	[Flags]
    public enum Flag
    {
		View   = 1,
		Add    = 2,
		Edit   = 4,
		Delete = 8,
    }
}
