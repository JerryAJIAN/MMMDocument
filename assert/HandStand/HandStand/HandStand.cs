﻿using System;
using MikuMikuPlugin;
using System.Windows.Forms;
using System.Drawing;
using DxMath;
namespace HandStand
{
    public class HandStand:ICommandPlugin   //命令插件接口
    {
        public Guid GUID    //每一个插件都需要的唯一识别码
        {
            get { return new Guid("bfeb8680-ae56-46b0-8a21-83f18e23dc5e"); }
        }

        public IWin32Window ApplicationForm //获取MMM本体窗口句柄
        {
            get; set;
        }

        public Scene Scene    //获取场景中的成员
        {
            get; set;
        }

        public string Description   //插件的描述
        {
            get { return "这是一个倒立的演示实验！"; }
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
            get { return "StandHand"; }
        }

        public string EnglishText   //英语环境下显示文本
        {
            get { return "StandHand"; }
        }

        public void Run(CommandArgs e)  //命令插件核心，按下按钮后执行的部分
        {
            MessageBox.Show(ApplicationForm, "这是一次倒立的实践！");
            BoneCollection bones = Scene.ActiveModel.Bones;
            for(int i = 0; i < bones.Count; i++)
            {
                if(bones[i].Name!= "全ての親")
                {
                    continue;
                }
                MotionLayerCollection layers = bones[i].Layers;
                for(int i2 = 0; i2 < layers.Count; i2++)
                {
                    MotionFrameCollection frameDates = layers[i2].Frames;
                    for(int f = 0; f < 30; f++)
                    {
                        MotionFrameData data = new MotionFrameData();
                        data.FrameNumber = f * 10;
                        data.Quaternion = Quaternion.RotationAxis(new Vector3(0, 0, 1), 6 * f);
                        
                        //Vector3 vector = frameDatas.GetFrame(f * 10).Position;
                        //vector.Y += 10;
                        frameDates.AddKeyFrame(data);
                    }
                }
            }
        }

        public void Dispose()   //释放插件内对象
        {

        }

    }
}
