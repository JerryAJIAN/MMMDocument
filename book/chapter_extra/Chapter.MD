### 附录

#### 第一次实践源代码

<span id="第一次实践源代码"></span>

````csharp
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
````

#### 官方RandBlink插件源码

<span id="官方RandBlink插件源码"></span>

````csharp
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using MikuMikuPlugin;
using DxMath;

namespace RandomBlinkPlugin
{
    public class RandomBlinkPlugin : ICommandPlugin
    {
        /// <summary>
        /// このプラグインのGUID
        /// </summary>
        public Guid GUID
        {
            get { return new Guid("693B76A5-589D-459D-844B-481489B0B41F"); }
        }

        /// <summary>
        /// メインフォーム
        /// MikuMikuMoving側から与えられます。
        /// ダイアログ表示やメッセージ表示に使用してください。
        /// </summary>
        public IWin32Window ApplicationForm { get; set; }

        /// <summary>
        /// シーンオブジェクト
        /// MikuMikuMoving側から与えられます。
        /// MikuMikuMovingのモデルやアクセサリといったオブジェクトにアクセスできます。
        /// </summary>
        public Scene Scene { get; set; }

        /// <summary>
        /// プラグインの名前や作者名、プラグインの説明
        /// </summary>
        public string Description
        {
            get { return "Random Blink Plugin v0.2 by Mogg"; }
        }

        /// <summary>
        /// ボタンに表示するアイコン画像(32x32)
        /// nullだとデフォルト画像が表示されます。
        /// </summary>
        public Image Image
        {
            get { return Properties.Resources.RandomBlink_L; }
        }

        /// <summary>
        /// 中コマンドバーに表示するアイコン画像(20x20)
        /// nullだとデフォルト画像が表示されます。
        /// </summary>
        public Image SmallImage
        {
            get { return Properties.Resources.RandomBlink; }
        }

        /// <summary>
        /// ボタンに表示するテキスト
        /// 日本語環境で表示されるテキストです。改行する場合は Environment.NewLineを使用してください。
        /// </summary>
        public string Text
        {
            get { return "まばたき" + Environment.NewLine + "ランダム登録"; }
        }

        /// <summary>
        /// ボタンに表示する英語テキスト
        /// 日本以外の環境で表示されるテキストです。
        /// </summary>
        public string EnglishText
        {
            get { return "Random" + Environment.NewLine + "Blink"; }
        }

        /// <summary>
        /// 実行
        /// ここにボタンが押されたときの処理を記述します。
        /// </summary>
        public void Run(CommandArgs e)
        {
            //モデルがアクティブかどうか
            if (Scene.ActiveModel != null)
            {
                using (RandomBlinkForm blinkDialog = new RandomBlinkForm())
                {
                    if (blinkDialog.ShowDialog(ApplicationForm) == DialogResult.OK)
                    {
                        //ダイアログから開始と終了フレームを取得する
                        long start, end;
                        if (blinkDialog.StartFrame < blinkDialog.EndFrame)
                        {
                            start = blinkDialog.StartFrame;
                            end = blinkDialog.EndFrame;
                        }
                        else
                        {
                            start = blinkDialog.EndFrame;
                            end = blinkDialog.StartFrame;
                        }

                        //まばたきモーフがあるかどうか
                        if (Scene.ActiveModel.Morphs["まばたき"] != null)
                        {
                            //まばたきモーフの作成先
                            List<MorphFrameData> morphlist = new List<MorphFrameData>();

                            //最小間隔
                            int min = (int)(Scene.KeyFramePerSec * 0.1);   //0.1秒
                            //最大間隔
                            int max = (int)(Scene.KeyFramePerSec * 3);   //3.0秒
                            long current = start;

                            //乱数
                            Random rand = new Random(Environment.TickCount);

                            //開始位置
                            current += rand.Next(min, max);

                            int count = 0;  //登録個数
                            while (current + (int)(0.8 * Scene.KeyFramePerSec) + 1 < end)
                            {
                                //まばたき登録

                                //最初のフレーム
                                morphlist.Add(new MorphFrameData(current, 0.0f));
                                current += (int)(0.2 * Scene.KeyFramePerSec);     //0.2秒

                                //2つ目のフレーム
                                morphlist.Add(new MorphFrameData(current, 1.0f));
                                current += (int)(0.2 * Scene.KeyFramePerSec);     //0.2秒

                                //3つ目のフレーム
                                morphlist.Add(new MorphFrameData(current, 1.0f));
                                current += (int)(0.2 * Scene.KeyFramePerSec);     //0.2秒

                                //4つ目のフレーム
                                morphlist.Add(new MorphFrameData(current, 0.0f));
                                current += 1;

                                count++;

                                //開始位置
                                current += rand.Next(min, max);
                            }

                            //作成したモーフフレームデータを登録する(同じフレームに既存キーフレームがあったら上書きされる)
                            Scene.ActiveModel.Morphs["まばたき"].Frames.AddKeyFrame(morphlist);

                            //結果をメッセージボックスで表示する
                            if (Scene.Language == "ja")
                                MessageBox.Show(ApplicationForm, count + "個のまばたきを登録しました", "登録完了", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            else
                                MessageBox.Show(ApplicationForm, count + " blink is added.", "Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            else
            {
                //モデルがアクティブでない場合はメッセージを出して終了
                if (Scene.Language == "ja")
                    MessageBox.Show(ApplicationForm, "モデルが選択された状態で実行してください", "確認", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                else
                    MessageBox.Show(ApplicationForm, "Please select a model.", "Note", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        /// <summary>
        /// プラグイン破棄処理
        /// もし解放しないといけないオブジェクトがある場合は、ここで解放してください。
        /// </summary>
        public void Dispose()
        {

        }
    }
}
````


#### MMM_EffectHeader文件

<span id="MMM_EffectHeader文件"></span>

````cpp
/********************************************************************
* MikuMikuMoving用エフェクトヘッダ
*********************************************************************/

#define MIKUMIKUMOVING

//////////////////////////////////////////////
// スキニング用行列
//////////////////////////////////////////////
//最大値
static const int MMM_MaxMatrices = 20;
//ライト数
static const int MMM_LightCount = 3;

float4x3 MMM_BoneMatrices[MMM_MaxMatrices]   : MMM_BONEMATRICES;	
float4	 MMM_BoneLocalQuat[MMM_MaxMatrices]  : MMM_BONELOCALQUATERNION;

bool	 MMM_LightEnables[MMM_LightCount]	 : LightEnables;
//影の濃さ
float	 MMM_ShadowDeep			: SHADOWDEEP;	
float	 MMM_ShadowDeepPlus		: SHADOWDEEP_POSITIVE;	
float	 MMM_ShadowDeepMinus	: SHADOWDEEP_NEGATIVE;

bool	 MMM_usetoon			: MMM_USETOON;
float	 MMM_modelsize			: MMM_MODELSIZE;


//パース変形用
float	 MMM_aspectratio		: MMM_ASPECTRATIO;
float	 MMM_cameradist			: MMM_CAMERADIST;
float	 MMM_startfov			: MMM_STARTFOV;
float	 MMM_endfov				: MMM_ENDFOV;
float	 MMM_fovcoefficient		: MMM_FOVCOEFFICIENT;
float	 MMM_shadowbias			: MMM_SHADOWBIAS;
bool	 MMM_IsDinamicProjection: MMM_ISDYNAMICFOV;

static const float MMM_Epsilon = 0.004f;

//////////////////////////////////////////////
// セルフシャドウ用デプスマップのテクスチャ
//////////////////////////////////////////////
texture MMM_SelfShadowDepthMap1   : MMM_SELFSHADOWDEPTHMAP1;
texture MMM_SelfShadowDepthMap2   : MMM_SELFSHADOWDEPTHMAP2;
texture MMM_SelfShadowDepthMap3   : MMM_SELFSHADOWDEPTHMAP3;
sampler MMM_SelfShadowSampler[MMM_LightCount] = {
	sampler_state {
		texture = <MMM_SelfShadowDepthMap1>;
		AddressU = BORDER;
		AddressV = BORDER;
		MINFILTER = LINEAR;
		MAGFILTER = LINEAR;
	},
	sampler_state {
		texture = <MMM_SelfShadowDepthMap2>;
		AddressU = BORDER;
		AddressV = BORDER;
		MINFILTER = LINEAR;
		MAGFILTER = LINEAR;
	},
	sampler_state {
		texture = <MMM_SelfShadowDepthMap3>;
		AddressU = BORDER;
		AddressV = BORDER;
		MINFILTER = LINEAR;
		MAGFILTER = LINEAR;
	}
};

//////////////////////////////////////////////
// トゥーンテクスチャ
//////////////////////////////////////////////
texture MMM_ToonTexture		: MMM_ToonTexture;
sampler MMM_ToonTexSampler = sampler_state {
    texture = <MMM_ToonTexture>;
	AddressU = CLAMP;
	AddressV = CLAMP;
    MINFILTER = LINEAR;
    MAGFILTER = LINEAR;
};

//////////////////////////////////////////////
// スキニング入力
//////////////////////////////////////////////
struct MMM_SKINNING_INPUT{
	float4 Pos				: POSITION;
	float4 BlendWeight		: BLENDWEIGHT;
	float4 BlendIndices		: BLENDINDICES;
	float3 Normal			: NORMAL;
	float2 Tex				: TEXCOORD0;
	float4 AddUV1			: TEXCOORD1;
	float4 AddUV2			: TEXCOORD2;
	float4 AddUV3			: TEXCOORD3;
	float4 AddUV4			: TEXCOORD4;
	float4 SdefC			: TEXCOORD5;
	float3 SdefR0			: TEXCOORD6;
	float3 SdefR1			: TEXCOORD7;
	float  EdgeWeight		: TEXCOORD8;
	float  Index			: PSIZE15;
};

//////////////////////////////////////////////
// スキニング用出力
//////////////////////////////////////////////
struct MMM_SKINNING_OUTPUT {
    float4 Position;	// 座標
    float3 Normal;		// 法線
};


//////////////////////////////////////////////
// クォータニオン->Matrix
//////////////////////////////////////////////
float4x4 MMM_QuaternionToMatrix(float4 quat)
{
	float4x4 mat;

	float xy = quat.x * quat.y;
	float zz = quat.z * quat.z;
	float wx = quat.w * quat.x;
	float wy = quat.w * quat.y;
	float yz = quat.y * quat.z;
	float wz = quat.w * quat.z;

	mat._11 = 1.0f - 2.0f * quat.y * quat.y - 2.0f * zz;
	mat._12 = 2.0f * xy + 2.0f * wz;
	mat._13 = 2.0f * quat.x * quat.z - 2.0f * wy;
	mat._14 = 0;

	mat._21 = 2.0f * xy - 2.0f * wz;
	mat._22 = 1.0f - 2.0f * quat.x * quat.x - 2.0f * zz;
	mat._23 = 2.0f * yz + 2.0f * wx;
	mat._24 = 0;

	mat._31 = 2.0f * quat.x * quat.z + 2.0f * wy;
	mat._32 = 2.0f * yz - 2.0f * wx;
	mat._33 = 1.0f - 2.0f * quat.x * quat.x - 2.0f * quat.y * quat.y;
	mat._34 = 0;

	mat._41 = 0;
	mat._42 = 0;
	mat._43 = 0;
	mat._44 = 1.0f;

	return mat;
}

//////////////////////////////////////////////
// Qlerp
//////////////////////////////////////////////
float4 MMM_Qlerp(float4 q1, float4 q2, float t)
{
	float4 quat, q1t;

	float rad = q1.w * q2.w + q1.x * q2.x + q1.y * q2.y + q1.z * q2.z;

	if (rad < 0)
	{
		t = -t;
		q1t = q1;
	}
	else
	{
		q1t = -q1;
	}
	quat = q1 + (q2 + q1t) * t;

	float n = sqrt(quat.x * quat.x + quat.y * quat.y + quat.z * quat.z + quat.w * quat.w);

	if (n > 0)
	{
		float s = 1.0f / n;
		quat = quat * s;
	}
	else
	{
		quat = float4(0, 0, 0, 1);
	}

	return quat;
}
float4 MMM_Qlerp_Identity(float4 q2, float t)
{
	float tt;
	float4 quat;

	if (q2.w < 0)
	{
		t = -t;
		tt = 1.0;
	}
	else
	{
		tt = -1.0;
	}
	quat.xyz = t * q2.xyz;
	quat.w = 1.0 + t * (q2.w + tt);

	float n = sqrt(quat.x * quat.x + quat.y * quat.y + quat.z * quat.z + quat.w * quat.w);

	if (n > 0)
	{
		float s = 1.0f / n;
		quat = quat * s;
	}
	else
	{
		quat = float4(0, 0, 0, 1.0);
	}

	return quat;
}
//////////////////////////////////////////////
// Slerp
//////////////////////////////////////////////
float4 MMM_Slerp(float4 q1, float4 q2, float t)
{
	float4 quat;

	float opposite, inverse;
	float tt = 1;
	float dot = q1.w * q2.w + q1.x * q2.x + q1.y * q2.y + q1.z * q2.z;

	if (dot < 0)
	{
		tt = -1;
		dot = -dot;
	}

	if (dot > 0.999999)
	{
		inverse = 1.0 - t;
		opposite = t * tt;
	}
	else
	{
		float acosf = acos(dot);
		float invSin = 1.0 / sin(acosf);

		inverse = sin((1.0 - t) * acosf) * invSin;
		opposite = sin(t * acosf) * invSin * tt;
	}

	return float4(inverse * q1.x + opposite * q2.x, inverse * q1.y + opposite * q2.y, inverse * q1.z + opposite * q2.z, inverse * q1.w + opposite * q2.w);
}
//////////////////////////////////////////////
// Multiply Quaternion
//////////////////////////////////////////////
float4 mul_quat(float4 q0, float4 q1)
{
	return float4(q0.w * q1.x + q0.x * q1.w + q0.y * q1.z - q0.z * q1.y,
				  q0.w * q1.y - q0.x * q1.z + q0.y * q1.w + q0.z * q1.x,
				  q0.w * q1.z + q0.x * q1.y - q0.y * q1.x + q0.z * q1.w,
				  q0.w * q1.w - q0.x * q1.x - q0.y * q1.y - q0.z * q1.z);
}
//////////////////////////////////////////////
// Dual Quaternion To Matrix
//////////////////////////////////////////////
float3x4 DQToMatrix(float4 Qn, float4 Qd)
{	
	float3x4 M;
	float len2 = dot(Qn, Qn);
	float w = Qn.x, x = Qn.y, y = Qn.z, z = Qn.w;
	float t0 = Qd.x, t1 = Qd.y, t2 = Qd.z, t3 = Qd.w;
		
	M[0][0] = w*w + x*x - y*y - z*z; M[0][1] = 2*x*y - 2*w*z; M[0][2] = 2*x*z + 2*w*y;
	M[1][0] = 2*x*y + 2*w*z; M[1][1] = w*w + y*y - x*x - z*z; M[1][2] = 2*y*z - 2*w*x; 
	M[2][0] = 2*x*z - 2*w*y; M[2][1] = 2*y*z + 2*w*x; M[2][2] = w*w + z*z - x*x - y*y;
	
	M[0][3] = -2*t0*x + 2*w*t1 - 2*t2*z + 2*y*t3;
	M[1][3] = -2*t0*y + 2*t1*z - 2*x*t3 + 2*w*t2;
	M[2][3] = -2*t0*z + 2*x*t2 + 2*w*t3 - 2*t1*y;
	
	M /= len2;
	
	return M;	
}

//////////////////////////////////////////////
// パース変形
//////////////////////////////////////////////
float MMM_GetDynamicFovValue(float dist)
{
	float d = pow(clamp(abs(dist), 0, MMM_cameradist) / MMM_cameradist, MMM_fovcoefficient);	//距離
	return lerp(MMM_startfov, MMM_endfov, 1.0 / (1.0 + exp(-d * 12.0 + 6.0)));	//FOVの計算
}
float4x4 MMM_DynamicFov(float4x4 PMatrix, float dist)
{
	float fov = MMM_GetDynamicFovValue(dist);

	// Projection行列に反映
	PMatrix._22 = 1.0f / tan( fov / 2.0f );
	PMatrix._11 = PMatrix._22 / MMM_aspectratio;

	return PMatrix;
}
float MMM_GetDynamicFovEdgeRate(float dist)
{
	return MMM_GetDynamicFovValue(dist) / MMM_endfov;
}

//////////////////////////////////////////////
// スキニング後の頂点座標と法線を取得する(BDEFのみ)
//////////////////////////////////////////////
MMM_SKINNING_OUTPUT MMM_BDEFPositionNormal(float4 Position, float3 Normal, float4 BlendWeight, float4 BlendIndices)
{
	MMM_SKINNING_OUTPUT Out = (MMM_SKINNING_OUTPUT)0;

	//ウェイトを反映
	float blendWeights[3] = (float[3])BlendWeight;

	float3 pos = 0.0f;
	float3 nml = 0.0f;

	float lastWeight = 0.0f;

	for (int i = 0; i < 3; i++)
	{
		lastWeight += blendWeights[i];
		pos += mul(Position, MMM_BoneMatrices[BlendIndices[i]]) * blendWeights[i];
		nml += mul(Normal, MMM_BoneMatrices[BlendIndices[i]]) * blendWeights[i];
	}
	lastWeight = 1.0f - lastWeight;
	pos += mul(Position, MMM_BoneMatrices[BlendIndices[3]]) * lastWeight;
	nml += mul(Normal, MMM_BoneMatrices[BlendIndices[3]]) * lastWeight;

	Out.Position = float4(pos, 1.0f);
	Out.Normal = nml;

	return Out;
}

//////////////////////////////////////////////
// スキニング後の頂点座標を取得する(BDEFのみ)
//////////////////////////////////////////////
float4 MMM_BDEFPosition(float4 Position, float4 BlendWeight, float4 BlendIndices)
{
	//ウェイトを反映
	float blendWeights[3] = (float[3])BlendWeight;

	float3 pos = 0.0f;

	float lastWeight = 0.0f;

	for (int i = 0; i < 3; i++)
	{
		lastWeight += blendWeights[i];
		pos += mul(Position, MMM_BoneMatrices[BlendIndices[i]]) * blendWeights[i];
	}
	lastWeight = 1.0f - lastWeight;
	pos += mul(Position, MMM_BoneMatrices[BlendIndices[3]]) * lastWeight;

	return float4(pos, 1.0f);
}

//////////////////////////////////////////////
// スキニング後の頂点座標と法線を取得する
//////////////////////////////////////////////
MMM_SKINNING_OUTPUT MMM_SkinnedPositionNormal_Internal(float4 Position, float3 Normal, float4 BlendWeight, float4 BlendIndices, float4 SdefC, float3 SdefR0, float3 SdefR1)
{
	MMM_SKINNING_OUTPUT Out = (MMM_SKINNING_OUTPUT)0;

	//ウェイトを反映
	float blendWeights[3] = (float[3])BlendWeight;

	float3 pos = 0.0f;
	float3 nml = 0.0f;

	if (SdefC.w >= 0)
	{
		float4 center = float4(SdefC.xyz, 1.0f);
		float4 r0 = float4(SdefR0, 1.0f);
		float4 r1 = float4(SdefR1, 1.0f);

		float3 pp0 = mul(r0, MMM_BoneMatrices[BlendIndices[0]]);
		float3 pp1 = mul(r1, MMM_BoneMatrices[BlendIndices[1]]);

		float3 prc = pp0 * blendWeights[0] + pp1 * blendWeights[1];

		float3 v1, diff;

		v1 = mul(center, MMM_BoneMatrices[BlendIndices[0]]);
		diff = (v1 - center) * blendWeights[0];

		v1 = mul(center, MMM_BoneMatrices[BlendIndices[1]]);
		diff += (v1 - center) * blendWeights[1];

		float3 trans = center + diff;
		trans = (trans + prc) * 0.5;

		float4 quat = MMM_Slerp(MMM_BoneLocalQuat[BlendIndices[0]], MMM_BoneLocalQuat[BlendIndices[1]], blendWeights[1]);
		float4x4 mat = MMM_QuaternionToMatrix(quat);
		
		pos = mul(Position - center, mat) * MMM_modelsize + trans;
		nml = mul(Normal, mat) * MMM_modelsize;
	}
	else
	{
		float lastWeight = 0.0f;

		for (int i = 0; i < 3; i++)
		{
			lastWeight += blendWeights[i];
			pos += mul(Position, MMM_BoneMatrices[BlendIndices[i]]) * blendWeights[i];
			nml += mul(Normal, MMM_BoneMatrices[BlendIndices[i]]) * blendWeights[i];
		}
		lastWeight = 1.0f - lastWeight;
		pos += mul(Position, MMM_BoneMatrices[BlendIndices[3]]) * lastWeight;
		nml += mul(Normal, MMM_BoneMatrices[BlendIndices[3]]) * lastWeight;
	}

	Out.Position = float4(pos, 1.0f);
	Out.Normal = nml;

	return Out;
}

//////////////////////////////////////////////
// スキニング後の頂点座標を取得する
//////////////////////////////////////////////
float4 MMM_SkinnedPosition_Internal(float4 Position, float4 BlendWeight, float4 BlendIndices, float4 SdefC, float3 SdefR0, float3 SdefR1)
{
	//ウェイトを反映
	float blendWeights[3] = (float[3])BlendWeight;

	float3 pos = 0.0f;

	if (SdefC.w >= 0)
	{
		float4 center = float4(SdefC.xyz, 1.0f);
		float4 r0 = float4(SdefR0, 1.0f);
		float4 r1 = float4(SdefR1, 1.0f);

		float3 pp0 = mul(r0, MMM_BoneMatrices[BlendIndices[0]]);
		float3 pp1 = mul(r1, MMM_BoneMatrices[BlendIndices[1]]);

		float3 prc = pp0 * blendWeights[0] + pp1 * blendWeights[1];

		float3 v1, diff;

		v1 = mul(center, MMM_BoneMatrices[BlendIndices[0]]);
		diff = (v1 - center) * blendWeights[0];

		v1 = mul(center, MMM_BoneMatrices[BlendIndices[1]]);
		diff += (v1 - center) * blendWeights[1];

		float3 trans = center + diff;
		trans = (trans + prc) * 0.5;

		float4 quat = MMM_Slerp(MMM_BoneLocalQuat[BlendIndices[0]], MMM_BoneLocalQuat[BlendIndices[1]], blendWeights[1]);
		float4x4 mat = MMM_QuaternionToMatrix(quat);

		pos = mul(Position - center, mat) * MMM_modelsize + trans;
	}
	else
	{
		float lastWeight = 0.0f;

		for (int i = 0; i < 3; i++)
		{
			lastWeight += blendWeights[i];
			pos += mul(Position, MMM_BoneMatrices[BlendIndices[i]]) * blendWeights[i];
		}
		lastWeight = 1.0f - lastWeight;
		pos += mul(Position, MMM_BoneMatrices[BlendIndices[3]]) * lastWeight;
	}

	return float4(pos, 1.0f);
}

//////////////////////////////////////////////
// エフェクト用(アクセサリ兼用)
//////////////////////////////////////////////
MMM_SKINNING_OUTPUT MMM_SkinnedPositionNormal(float4 Position, float3 Normal, float4 BlendWeight, float4 BlendIndices, float4 SdefC, float3 SdefR0, float3 SdefR1)
{
	MMM_SKINNING_OUTPUT Out;

	if (MMM_usetoon)
	{
		Out = MMM_SkinnedPositionNormal_Internal(Position, Normal, BlendWeight, BlendIndices, SdefC, SdefR0, SdefR1);
	}
	else
	{
		Out.Position = Position;
		Out.Normal = Normal;
	}
	return Out;
}
//////////////////////////////////////////////
// エフェクト用(アクセサリ兼用)
//////////////////////////////////////////////
MMM_SKINNING_OUTPUT MMM_SkinninedPositionNormal(float4 Position, float3 Normal, float4 BlendWeight, float4 BlendIndices, float4 SdefC, float3 SdefR0, float3 SdefR1)
{
	return MMM_SkinnedPositionNormal(Position, Normal, BlendWeight, BlendIndices, SdefC, SdefR0, SdefR1);
}
//////////////////////////////////////////////
// エフェクト用(アクセサリ兼用)
//////////////////////////////////////////////
float4 MMM_SkinnedPosition(float4 Position, float4 BlendWeight, float4 BlendIndices, float4 SdefC, float3 SdefR0, float3 SdefR1)
{
	float4 Out;

	if (MMM_usetoon)
	{
		Out = MMM_SkinnedPosition_Internal(Position, BlendWeight, BlendIndices, SdefC, SdefR0, SdefR1);
	}
	else
	{
		Out = Position;
	}
	return Out;
}

//////////////////////////////////////////////
// 法線・深度のパック
//////////////////////////////////////////////
float4	MMM_PackDepthNormal( float z, float3 normal )
{
	//float4	output;

	//float packz, packw;
	//packw = modf(z * 255.0, packz);
	//output = float4( normal.x, normal.y, packz / 255.0, packw );
	
	return float4(z, normal);
}
//////////////////////////////////////////////
// 深度のパック
//////////////////////////////////////////////
float4 MMM_PackDepth( float z )
{
	//float packz, packw;
	//packw = modf(z * 255.0, packz);

	//xy : 2乗、zw : 深度
	//return float4( packz / 255.0, packw, 0, 1.0 );

	return float4(z, 1.0f, 1.0f, 1.0f);
}
//////////////////////////////////////////////
// 深度のパック(浮動小数点)
//////////////////////////////////////////////
float4 MMM_PackDepth_F( float z )
{
	return float4( z, z * z, 0, 1.0 );
}
//////////////////////////////////////////////
// 詳細深度のパック
//////////////////////////////////////////////
float4 MMM_PackDetailedDepth( float z )
{
	float4 depth = float4(0, 0, 255.0f, 255.0f);
	depth.g = modf(z * 255.0f, depth.r);
	depth.b *= modf(depth.g * 255.0f, depth.g);
	return depth / 255.0f;
}

//////////////////////////////////////////////
// 法線・深度のアンパック
//////////////////////////////////////////////
void MMM_UnpackDepthNormal( out float z, out float3 normal, float4 input )
{
	//normal.xy = input.xy;
	//normal.z = -sqrt( 1 - input.x * input.x - input.y * input.y );
	//z = input.z + input.w / 255.0;
	z = input.x;
	normal = float3(input.y, input.z, input.w);
}

//////////////////////////////////////////////
// 深度のアンパック
//////////////////////////////////////////////
float MMM_UnpackDepth(float4 input )
{
	//return input.x + input.y / 255.0;
	return input.x;
}
//////////////////////////////////////////////
// 深度のアンパック(浮動小数点)
//////////////////////////////////////////////
float MMM_UnpackDepth_F(float4 input )
{
	return input.x;
}
float MMM_UnpackDepthSq_F(float4 input )
{
	return input.y;
}
//////////////////////////////////////////////
// 詳細深度のアンパック
//////////////////////////////////////////////
float MMM_UnpackDetailedDepth(float4 input )
{
	return input.r + (input.g + input.b / 255.0f) / 255.0f;
}
//////////////////////////////////////////////
// 法線のアンパック
//////////////////////////////////////////////
float3 MMM_UnpackNormal(float4 input )
{
	float3 normal;
	normal.xy = input.xy;
	normal.z = -sqrt( 1 - input.x * input.x - input.y * input.y );

	return normal;
}

//////////////////////////////////////////////
// トゥーンの色を取得する
//////////////////////////////////////////////
float3 MMM_GetToonColor(float4 MaterialToon, float3 Normal, float3 LightDirection0, float3 LightDirection1, float3 LightDirection2)
{
	float3 shadow = float3(1,1,1);
	float3 LightDirection[MMM_LightCount];
	float LightNormal;
	float3 leapcolor = lerp(MaterialToon * MMM_ShadowDeepPlus, 1.0, 1.0 - MMM_ShadowDeepMinus);

	LightDirection[0] = LightDirection0;
	LightDirection[1] = LightDirection1;
	LightDirection[2] = LightDirection2;
	for (int i = 0; i < MMM_LightCount; i++)
	{
		if (MMM_LightEnables[i])
		{
			LightNormal = dot( Normal, -LightDirection[i] );
			shadow *= lerp(leapcolor, float3(1,1,1), saturate(LightNormal * 16 + 0.5));
		}
	}

	return shadow;
}

//////////////////////////////////////////////
// セルフシャドウ有効時のトゥーンの色を取得する
//////////////////////////////////////////////
float3 MMM_GetSelfShadowToonColor(float4 MaterialToon, float3 Normal, float4 LightUV1, float4 LightUV2, float4 LightUV3, uniform bool useSoftShadow, uniform bool useToon)
{
	float3 color = 1;
	float smz, tm;
	float2 texcoord;
	float4 uv[3];
	float4 d;
	float3 tt;

	uv[0] = LightUV1;
	uv[1] = LightUV2;
	uv[2] = LightUV3;

	if (useToon)
	{
		//キャラクタのセルフシャドウ処理
		if (useSoftShadow)
		{
			float4 depth;
			float dd, variance, dx, depth_sq, md, lit_fact;
			[unroll(3)]
			for (int i = 0; i < MMM_LightCount; i++)
			{
				if (MMM_LightEnables[i])
				{
					dd = uv[i].z;
					texcoord.xy = (1.0f + uv[i].xy) * 0.5f;
					tt.xy = 1.0f - saturate((uv[i].xy * uv[i].xy - 0.8f) / 0.2f);

					depth = tex2D(MMM_SelfShadowSampler[i], texcoord);
					dx = MMM_UnpackDepth_F(depth);

					lit_fact = (dd <= dx);

					depth_sq = dx * dx;
					variance = saturate(MMM_UnpackDepthSq_F(depth) + MMM_Epsilon - depth_sq) * dd * dd;
					md = dd - dx;
					smz = variance / (variance + md * md);

					smz = 1.0 - max(lit_fact, pow(smz, 2000));
					tm = 1.0 - smz * MMM_ShadowDeepMinus * tt.x * tt.y;
					color *= lerp(tm, 1.0, MaterialToon * MMM_ShadowDeepPlus);
				}
			}
		}
		else
		{
			[unroll(3)]
			for (int i = 0; i < MMM_LightCount; i++)
			{
				if (MMM_LightEnables[i])
				{
					tm = uv[i].z;
					uv[i].xy = uv[i].xy / uv[i].w;
					texcoord.xy = (1.0f + uv[i].xy) * 0.5f;
					tt.xy = 1.0f - saturate((uv[i].xy * uv[i].xy - 0.8f) / 0.2f);

					d = tex2D(MMM_SelfShadowSampler[i], texcoord);
					smz = MMM_UnpackDepth(d) + 0.00002;

					if (tm > smz)
					{
						tm = 1.0 - saturate((tm - smz) * 6000.0f) * MMM_ShadowDeepMinus * tt.x * tt.y;
						color *= lerp(tm, 1.0, MaterialToon * MMM_ShadowDeepPlus);
					}
				}
			}
		}
	}
	else
	{
		
		
		// アクセサリのセルフシャドウ処理
		if (useSoftShadow)
		{
			float4 depth;
			float dd, variance, dx, depth_sq, md, lit_fact;
			for (int i = 0; i < 3; i++)
			{
				if (MMM_LightEnables[i])
				{
					dd = uv[i].z;
					texcoord.xy = (1.0f + uv[i].xy) * 0.5f;
					tt.xy = 1.0f - saturate((uv[i].xy * uv[i].xy - 0.8f) / 0.2f);

					depth = tex2D(MMM_SelfShadowSampler[i], texcoord);
					dx = MMM_UnpackDepth_F(depth);

					lit_fact = (dd <= dx);

					depth_sq = dx * dx;
					variance = saturate(MMM_UnpackDepthSq_F(depth) + MMM_Epsilon - depth_sq) * dd * dd;
					md = dd - dx;
					smz = variance / (variance + md * md);

					smz = 1.0 - max(lit_fact, pow(smz, 2000));
					tm = tt.x * tt.y;
					color *= 1.0f - (MMM_ShadowDeep * smz * tm);
				}
			}
		}
		else
		{
			for (int i = 0; i < 3; i++)
			{
				if (MMM_LightEnables[i])
				{
					tt = uv[i].xyz;
					tt.xy = tt.xy / uv[i].w;
					texcoord = (1.0f + tt.xy) * 0.5f;
					tt.xy = 1.0f - saturate((tt.xy * tt.xy - 0.8f) / 0.2f);
			
					d = tex2D(MMM_SelfShadowSampler[i], texcoord);
					smz = MMM_UnpackDepth(d) + 0.00002;

					if (tt.z > smz)
					{
						tm = tt.x * tt.y;
						color *= 1.0f - (MMM_ShadowDeep * tm);
					}
					
				}
			}
		}
	}
	return color;
}

````