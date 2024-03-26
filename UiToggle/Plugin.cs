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

        private ICommandManager CommandManager { get; init; }
        private IGameGui GameGui { get; set; }
        private IChatGui ChatGui { get; set; }
        private IPluginLog PluginLog { get; init; }

        private bool toggle = false;

        public Plugin(
            [RequiredVersion("1.0")] ICommandManager commandManager,
            [RequiredVersion("1.0")] IGameGui gameGui,
            [RequiredVersion("1.0")] IChatGui chatGui,
            [RequiredVersion("1.0")] IPluginLog pluginLog)
        {
            this.CommandManager = commandManager;
            this.GameGui = gameGui;
            this.ChatGui = chatGui;
            this.PluginLog = pluginLog;

            this.CommandManager.AddHandler(CommandName, new CommandInfo(OnCommand));
        }

        public void Dispose()
        {
            this.CommandManager.RemoveHandler(CommandName);
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
            var uiModule = (UIModule*)GameGui.GetUIModule();
            var raptureAtkModule = uiModule->GetRaptureAtkModule();
            raptureAtkModule->SetUiVisibility(!raptureAtkModule->IsUiVisible);
        }
    }
}
