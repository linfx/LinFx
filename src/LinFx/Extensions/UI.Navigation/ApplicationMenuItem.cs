using System.Diagnostics.CodeAnalysis;

namespace LinFx.Extensions.UI.Navigation
{
    public class ApplicationMenuItem
    {
        /// <summary>
        /// Unique name of the menu in the application.
        /// </summary>
        [NotNull]
        public string Name { get; }

        /// <summary>
        /// Display name of the menu item.
        /// </summary>
        [NotNull]
        public string DisplayName { get; set; }

        /// <summary>
        /// The Display order of the menu.
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// The URL to navigate when this menu item is selected.
        /// </summary>
        [CanBeNull]
        public string Url { get; set; }

        /// <summary>
        /// Icon of the menu item if exists.
        /// </summary>
        [CanBeNull]
        public string Icon { get; set; }

        /// <summary>
        /// Target of the menu item. Can be null, "_blank", "_self", "_parent", "_top" or a frame name for web applications.
        /// </summary>
        [CanBeNull]
        public string Target { get; set; }

        public ApplicationMenuItem(
            [NotNull] string name,
            [NotNull] string displayName,
            string url = default, 
            int order = 100, 
            string icon = default, 
            string target = default)
        {
            Name = name;
            DisplayName = displayName;
            Url = url;
            Order = order;
            Icon = icon;
            Target = target;
        }
    }
}