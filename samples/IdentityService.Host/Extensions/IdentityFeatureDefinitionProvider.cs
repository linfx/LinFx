using LinFx.Extensions.Features;

namespace IdentityService;

/// <summary>
/// 功能套餐
/// </summary>
public class FunFeatureDefinitionProvider : FeatureDefinitionProvider
{
    public const string GroupName = "Fun";
    public const string Sip = GroupName + ".Sip";
    public const string SipAlarm = GroupName + ".SipAlarm";
    public const string SmsAlarm = GroupName + ".SmsAlarm";

    public override void Define(IFeatureDefinitionContext context)
    {
        var group = context.AddGroup(GroupName, L[GroupName]);

        group.AddFeature(
            Sip,
        //valueType: new ToggleStringValueType()
        displayName: L[Sip]);

        group.AddFeature(
            SipAlarm
        //valueType: new ToggleStringValueType()
        );

        group.AddFeature(
            SmsAlarm,
            defaultValue: false.ToString().ToLowerInvariant() //Optional, it is already false by default
                                                              //valueType: new FreeTextStringValueType(new NumericValueValidator(1, 1000))
        );
    }
}

/// <summary>
/// 照片套餐
/// </summary>
public class PicFeatureDefinitionProvider : FeatureDefinitionProvider
{
    public const string GroupName = "Pic";
    public const string CloudStorage = GroupName + ".CloudStorage";

    public override void Define(IFeatureDefinitionContext context)
    {
        var group = context.AddGroup(GroupName, L[GroupName]);

        group.AddFeature(
            CloudStorage,
            defaultValue: "0"
        //valueType: new FreeTextStringValueType(new NumericValueValidator(0, 10))
        );
    }
}