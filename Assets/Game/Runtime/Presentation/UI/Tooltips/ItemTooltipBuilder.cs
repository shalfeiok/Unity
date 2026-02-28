using System.Text;
using Game.Domain.Poe.Items;
using Game.Presentation.UI.Localization;

namespace Game.Presentation.UI.Tooltips
{
    public sealed class ItemTooltipBuilder
    {
        private readonly ILocalizationService _loc;

        public ItemTooltipBuilder(ILocalizationService loc)
        {
            _loc = loc;
        }

        public TooltipModel Build(GeneratedPoeItem item)
        {
            var model = new TooltipModel
            {
                Title = _loc.TranslateFormat("tooltip.item.title", item.ItemBase?.Id ?? _loc.Translate("tooltip.empty")),
                Subtitle = _loc.TranslateFormat("tooltip.item.ilvl", item.ItemLevel)
            };

            var meta = new TooltipSection();
            meta.Header = "";
            meta.Lines.Add(_loc.TranslateFormat("tooltip.item.quality", item.Quality));
            meta.Lines.Add(_loc.TranslateFormat("tooltip.item.sockets", item.SocketColors.Count));
            model.Sections.Add(meta);

            var implicits = new TooltipSection { Header = _loc.Translate("tooltip.section.implicits") };
            AppendMods(implicits, item.Implicits);
            model.Sections.Add(implicits);

            var modifiers = new TooltipSection { Header = _loc.Translate("tooltip.section.modifiers") };
            AppendMods(modifiers, item.Mods);
            model.Sections.Add(modifiers);

            var links = new TooltipSection { Header = _loc.Translate("tooltip.section.links") };
            for (int i = 0; i < item.LinkGroups.Count; i++)
            {
                var group = item.LinkGroups[i];
                var line = new StringBuilder();
                for (int n = 0; n < group.Length; n++)
                {
                    if (n > 0) line.Append('-');
                    line.Append(group[n]);
                }
                links.Lines.Add(line.ToString());
            }

            if (links.Lines.Count == 0)
                links.Lines.Add(_loc.Translate("tooltip.empty"));
            model.Sections.Add(links);

            return model;
        }

        private void AppendMods(TooltipSection section, System.Collections.Generic.IReadOnlyList<GeneratedPoeMod> mods)
        {
            for (int i = 0; i < mods.Count; i++)
            {
                var mod = mods[i];
                section.Lines.Add($"{mod.Definition.Id}: {mod.RolledValue:0.##}");
            }

            if (section.Lines.Count == 0)
                section.Lines.Add(_loc.Translate("tooltip.empty"));
        }
    }
}
