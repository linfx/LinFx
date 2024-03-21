using LinFx.Extensions.Features;

namespace IdentityService;

public class IdentityFeatureDefinitionProvider : FeatureDefinitionProvider
{
    public const string GroupName = "Fun";
    public const string SocialLogins = GroupName + ".SocialLogins";
    public const string EmailSupport = GroupName + ".EmailSupport";
    public const string DailyAnalysis = GroupName + ".DailyAnalysis";
    public const string UserCount = GroupName + ".UserCount";
    public const string ProjectCount = GroupName + ".ProjectCount";
    public const string BackupCount = GroupName + ".BackupCount";

    public override void Define(IFeatureDefinitionContext context)
    {
        var group = context.AddGroup(GroupName);

        group.AddFeature(
            SocialLogins
            //valueType: new ToggleStringValueType()
        );

        group.AddFeature(
            EmailSupport
            //valueType: new ToggleStringValueType()
        );

        group.AddFeature(
            DailyAnalysis,
            defaultValue: false.ToString().ToLowerInvariant() //Optional, it is already false by default
            //valueType: new ToggleStringValueType()
        );

        //group.AddFeature(
        //    UserCount,
        //    defaultValue: "1"
        //    //valueType: new FreeTextStringValueType(new NumericValueValidator(1, 1000))
        //);

        //group.AddFeature(
        //    ProjectCount,
        //    defaultValue: "1"
        //    //valueType: new FreeTextStringValueType(new NumericValueValidator(1, 10))
        //);

        //group.AddFeature(
        //    BackupCount,
        //    defaultValue: "0"
        //    //valueType: new FreeTextStringValueType(new NumericValueValidator(0, 10))
        //);
    }
}
