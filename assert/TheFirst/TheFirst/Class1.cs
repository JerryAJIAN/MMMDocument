using System;
using System.Drawing;
using System.Windows.Forms;
using MikuMikuPlugin;

namespace TheFirst
{
    public class ShowTestDialog : ICommandPlugin    //命令插件接口
    {
        public Guid GUID    //每一个插件都需要的唯一识别码
        {
            get { return new Guid("085e1ce9-0551-4840-b065-5016783296d7"); }
        }

        public IWin32Window ApplicationForm //获取MMM本体窗口句柄
        {
            get;set;
        }

        public Scene Scene    //获取场景中的成员
        {
            get; set;
        }

        public string Description   //插件的描述
        {
            get { return "这是一个创建插件的演示实验！"; }
        }

        public Image Image  //插件图标
        {
            get { return null; }
        }

        public Image SmallImage //插件命令栏图标
        {
            get { return null; }
        }

        public string Text  //日语环境下按钮显示文本
        {
            get { return "ShowTestDialog"; }
        }

        public string EnglishText   //英语环境下显示文本
        {
            get { return "ShowTestDialog"; }
        }

        public void Run(CommandArgs e)  //命令插件核心，按下按钮后执行的部分
        {
            MessageBox.Show(ApplicationForm, "这是一次创建插件的实践！");
        }

        public void Dispose()   //释放插件内对象
        {

        }

    }
}
