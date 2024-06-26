﻿using LinFx;
using LinFx.Extensions.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Reflection;

namespace Microsoft.AspNetCore.Mvc.Abstractions;

public static class ActionDescriptorExtensions
{
    public static ControllerActionDescriptor AsControllerActionDescriptor(this ActionDescriptor actionDescriptor)
    {
        if (!actionDescriptor.IsControllerAction())
            throw new LinFxException($"{nameof(actionDescriptor)} should be type of {typeof(ControllerActionDescriptor).AssemblyQualifiedName}");

        return (ControllerActionDescriptor)actionDescriptor;
    }

    public static MethodInfo GetMethodInfo(this ActionDescriptor actionDescriptor) => actionDescriptor.AsControllerActionDescriptor().MethodInfo;

    public static Type GetReturnType(this ActionDescriptor actionDescriptor) => actionDescriptor.GetMethodInfo().ReturnType;

    public static bool HasObjectResult(this ActionDescriptor actionDescriptor) => ActionResultHelper.IsObjectResult(actionDescriptor.GetReturnType());

    public static bool IsControllerAction(this ActionDescriptor actionDescriptor) => actionDescriptor is ControllerActionDescriptor;

    public static bool IsPageAction(this ActionDescriptor actionDescriptor) => actionDescriptor is PageActionDescriptor;
}
