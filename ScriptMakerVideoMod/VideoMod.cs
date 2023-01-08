using System.Linq;
using ScriptMaker.Entry.Block;
using ScriptMaker.Entry.Block.Contexts.Dialog;
using ScriptMaker.Program.Data;
using ScriptMaker.Program.Mod;
using UnityEngine;

namespace ScriptMakerVideoMod
{
    [Mod("Video")]
    [AddDependency("MakerCore")]
    public class VideoMod
    {
        public VideoMod()
        {
            BlockHandler.RegisterBlock(typeof(VideoBlock), new Option("Context", "VideoContext")
                .Append("Video", "")
                .Append("Chromakey", "False"), typeof(VideoBlockContextEditDialog), "Video");

            GameObject k = null;
            ModEvent.Context.OpenDialogEvent += dialog =>
            {
                if (dialog.Context.String == "DelayContext")
                {
                    dialog.Next();
                    dialog.AddText("Info", "여기에 비디오가 끝나면 자동으로 넘어갈지를 결정할 내용이 추가될수도 있고 안될수도 있습니다.", transform: x =>
                    {
                        k = x.gameObject;
                        x.gameObject.SetActive(false);
                    }, height: 300);
                }
            };
            ModEvent.Context.CreateDropdownEvent += (dialog, dropdown, menus) =>
            {
                if (dialog.Context.String == "DelayContext")
                {
                    menus.Add(("Video", "Video"));
                }
            };
            ModEvent.Context.OnDropdownMenuChangedEvent += (dialog, dropdown, menu) =>
            {
                if (dialog.Context.String == "DelayContext" && k != null)
                {
                    k.SetActive(menu == "Video");
                }
            };
        }
    }

    public class VideoBlock : BaseBlock
    {
        protected override string Text => $"Video: {Context["Video"].String()}";
        protected override Color Color => Color.white;
    }

    public class VideoBlockContextEditDialog : ContextEditDialog
    {
        protected override void Initialize()
        {
            AddSingleLineInputField("VideoName", x => Context["Video"].First().Value = x, Context["Video"].String());
            Next();
            AddCheckbox("UsingChromakey", "Chromakey", x => Context["Chromakey"].First().Value = x.ToString(), Context["Chromakey"].Bool());
            base.Initialize();
        }
    }
}