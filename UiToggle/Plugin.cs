using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using FFXIVClientStructs.FFXIV.Client.UI;

namespace UiToggle
{
    public sealed class Plugin : IDalamudPlugin
    {
        public string Name => "UI Toggle Command";
        private const string CommandName = "/uitoggle";

        [PluginService] internal static ICommandManager CommandManager { get; private set; } = null!;
        [PluginService] internal static IGameGui GameGui { get; private set; } = null!;
        [PluginService] internal static IChatGui ChatGui { get; private set; } = null!;
        [PluginService] internal static IPluginLog PluginLog { get; private set; } = null!;

        private bool toggle = false;

        public Plugin()
        {
            CommandManager.AddHandler(CommandName, new CommandInfo(OnCommand));
        }

        public void Dispose()
        {
            CommandManager.RemoveHandler(CommandName);
        }

        private void OnCommand(string command, string args)
        {
            if (args == "show")
            {
                if (GameGui.GameUiHidden) toggleUI();
            }
            else if (args == "hide")
            {
                if (!GameGui.GameUiHidden) toggleUI();
            }
            else if (args == "")
            {
                toggleUI();
            }
            else
            {
                ChatGui.Print($"Usage: {CommandName} [show|hide]");
            }
        }

        private unsafe void toggleUI()
        {
            var uiModule = (UIModule*)GameGui.GetUIModule().Address;
            var raptureAtkModule = uiModule->GetRaptureAtkModule();
            raptureAtkModule->SetUiVisibility(!raptureAtkModule->IsUiVisible);
        }
    }
}
