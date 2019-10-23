>本章目录
[TOC]

### 第一章 MMM插件介绍

>&emsp;&emsp;在实践编写插件之前，我们需要对MMM插件有个大致的了解，比如它是干什么的，它由哪几部分组成，这些部分又是干什么的。
>&emsp;&emsp;在了解了MMM插件的基础组成后，这一章还会带大家实践创建一个MMM插件，初步参与到MMM插件的制作当中！

#### 第一节 插件的作用

&emsp;&emsp;插件有什么用？插件，顾名思义就是安插在原软件上的一段外来程序。因此，插件最常见的两个用处就是扩充软件功能和修BUG。
&emsp;&emsp;修BUG暂且不提，在这里，我们主要谈论的是扩充功能这一用处。
&emsp;&emsp;我想做一个数十人跳舞的场景，难道要加载数十个模型进去吗？
&emsp;&emsp;我想要一场带有物理效果樱花雨，难道我需要不停地在场景上放置樱花瓣模型？
&emsp;&emsp;我想要在场景中加入一些自定义动画，难道只能后期画大量时间做特效？
&emsp;&emsp;其实，在MMM中，这些功能都可依靠编写插件来实现。
&emsp;&emsp;不管是用来节省操作的一键命令插件还是用于实现新奇特效的插件，甚至是直接取代原有Shader的插件都可以被编写出来。
&emsp;&emsp;如果能有方便快捷的插件辅助，制作自己喜欢的视频想必也会更加轻松吧。

#### 第二节 MMM留给我们的魔法咒语

&emsp;&emsp;如果把编写插件比作施展魔法，那MMM留给我们的接口就像是用于释放魔力的咒语。只有掌握和正确使用这些“咒语”，我们才能成功施展魔法。
&emsp;&emsp;虽然也存在着类似“黑魔法”这种可以跨越接口限制为所欲为的方法，但在这里先不作介绍，毕竟，玩弄“黑魔法”很容易被反噬（出BUG）可是常识啊！
&emsp;&emsp;先介绍魔……不对，插件的两个大类：**命令插件**（ICommandPlugin）和**常驻插件**（IResidentPlugin）：

|类型|描述|接口|
|---|---|---|
|命令插件|按下按钮后开始执行功能，执行完毕之后停止工作|ICommandPlugin|
常驻插件|实时生效，每一帧都会在工作|IResidentPlugin

&emsp;&emsp;除了分为这两个大类外，还有一些用于辅助功能的接口：
类型|描述|接口
|---|---|---|
保存数据|将插件的数据保存到视频工程文件中|ICanSavePlugin
用户控制|用于用户控制|IHaveUserControl

&emsp;&emsp;那么，这些魔法咒语可以帮我们实现一些什么样的功能呢？主要概括起来就是对模型，附件，效果，相机等进行操作。下面列出具体的操作范围：

* 模型
  * 材料
  * 表示枠
  * 骨骼
    * 层
      * 关键帧
  * 属性
    * 关键帧
* 附件
  * 层
    * 关键帧
* 效果
  * 关键帧
* 相机
  * 层
    * 关键帧
* 灯光
  * 关键帧
* 总体属性
  * 关键帧
* 字幕
* 音频
* 书签
* 屏幕对象

&emsp;&emsp;以上内容就是MMM预留给我们的可供操作的啦！嗯？觉得有点少？没关系，等到这些内容熟悉之后，我可能还会放点“黑魔法”出来，如果你有信心对付BUG的话，那么对MMM所有内容为所欲为也是可以的哦~
&emsp;&emsp;接下来，我们继续讲讲命令插件和常驻插件的具体构成吧！

#### 第三节 命令插件

&emsp;&emsp;在命令插件中，必须使用接口ICommandPlugin，只有这样MMM才能将你识别为命令插件。

方法或属性|描述
|---|---|
|Guid GUID{get;}|插件的唯一识别标识，必须确保和其他的插件不同，建议随机生成！|
|IWin32Window ApplicationForm { get; set; }|获取MMM窗口句柄，用来用于弹提示消息框之类的，不必要掌握，会抄个弹窗写法就行|
|Scene Scene{get;set;}|相当重要的属性，用来获取MMM中的场景对象，以后打交道最多的值之一，必须熟悉它的结构和作用|
|string Description{get;}|插件介绍属性，你只需要把自己插件的介绍写在里面就行了，不是很重要
|Image Image{get;}|插件按钮的图标，可以留空，如果有的话，建议使用32*32大小|
|Image SmallImage{get;}|显示在命令栏中的图标，可以留空，如果有的话，建议使用32*32大小|
|string Text|显示在按钮上的文字（日语环境）|
|string EnglishText|显示在按钮上的文字（英语环境）|
|Void Run(CommandArgs e)|按下按钮后执行的操作，最重要的地方，命令插件相关功能全部在这里实现|
|Void Dispose()|非常重要的部分，在撤除插件时，你需要把所有插件有关的对象全部销毁掉，避免插件撤销后发生错误|

&emsp;&emsp;在命令插件中，你需要执行的方法必须在Run中完成，当使用者点击按钮后，MMM将调用Run中的内容。

#### 第四节 常驻插件

&emsp;&emsp;在常驻插件中，必须使用接口IResidentPlugin，只有这样MMM才能将你识别为常驻插件。

|方法或属性|描述|
|---|---|
|Guid GUID{get;}|插件的唯一识别标识，必须确保和其他的插件不同，建议随机生成！|
|IWin32Window ApplicationForm { get; set; }|获取MMM窗口句柄，用来弹提示消息框之类的，不必要掌握，会抄个弹窗写法就行|
|Scene Scene{get;set;}|相当重要的属性，用来获取MMM中的场景对象，以后打交道最多的值之一，必须熟悉它的结构和作用|
|string Description{get;}|插件介绍属性，你只需要把自己插件的介绍写在里面就行了，不是很重要|
|Image Image{get;}|插件按钮的图标，可以留空，如果有的话，建议使用32*32大小|
|Image SmallImage{get;}|显示在命令栏中的图标，可以留空，如果有的话，建议使用32*32大小|
|string Text|显示在按钮上的文字（日语环境）|
|string EnglishText|显示在按钮上的文字（英语环境）|
|Initialize()|初始化函数，很重要，当常驻插件被载入时执行该函数中的内容|
|Update(float, float)|对于常驻插件来说，这里是最重要的地方。每一帧都会调用的函数，如果要做长效类的工作的话将方法放在这里|
|Enabled()|启用插件时调用|
|Disabled()|禁用插件时调用|
|void Dispose()|非常重要的部分，在撤除插件时，你需要把所有插件有关的对象全部销毁掉，避免插件撤销后发生错误|

&emsp;&emsp;需要注意的是，务必在Update期间内对Scene内容进行获取和操作内容，在Update之间对Scene进行增删内容，避免插件与本体，插件与插件之间产生冲突。

#### 第五节 其他的几个接口

&emsp;&emsp;接下来介绍其他的几个接口，其中IHaveUserControl和ICanSavePlugin两个也是比较常用的。
&emsp;&emsp;首先是ScreenObject接口，它可以在图像中获取到鼠标的位置，并且转化为操作窗口中的位置（2D/3D）。
&emsp;&emsp;在视频生成中这个接口不会有效，但是可以借助这个接口实现编辑过程中的显示，比如在3D场景中绘制一个摄像机范围框，让K镜头更加方便。

|类|说明|
|---|---|
|ScreenImage_2D|在2D屏幕中定位图像|
|ScreenImage_3D|在3D场景中定位图像|

&emsp;&emsp;生成ScreenObject之后，将其添加至Scene.ScreenObject中即可。在插件撤出时记得销毁插件创建的ScreenObject。

&emsp;&emsp;其次是数据保存接口（ICanSavePlugin），该接口可以将插件的数据保存至MMM的工程文件，下一次载入工程后可以读取数据还原设置。

|方法|说明|
|---|---|
|void OnLoadProject(Stream)|在工程被载入时调用该函数，其中Stream是保存到工程文件中的数据流|
|Stream OnSaveProject()|保存时调用，将插件需要保存的数据作为数据流返回|

&emsp;&emsp;另外，这里的数据流不需要在前文中提到的Dispose方法中释放。

&emsp;&emsp;最后，是用户控制接口（IHaveUserControl）。通过用户控制接口，我们可以实现自定义控件，也就是自己重新创建UI界面供用户操作，非常常用的一个接口。
&emsp;&emsp;该接口只有一个方法，UserControl CreateControl()，将自定义控件作为返回值返回即可。

#### 第六节 动手创建一个小插件吧
&emsp;&emsp;我的电脑系统为Win10，使用的IDE为Visual Studio 2017，如果使用不同环境的话请根据实际情况自行调整。
&emsp;&emsp;在VS中安装.Net开发，即可开始创建工程。
&emsp;&emsp;打开VS，创建新项目，C#，类库（.Net Framework）。

![创建新工程](/chapter_1/assert/创建插件工程.png)

&emsp;&emsp;刚刚建立好的工程一片空白，在开始编写插件之前，我们还需要做一些其他的准备工作。
&emsp;&emsp;打开路径：MMM安装目录\System\x64\，如果是32位则为x32，将其中的MikuMikuPlugin.dll文件拷贝到一个你觉得方便查找的地方。

![需要引用文件的位置](/chapter_1/assert/引用文件位置.png)

&emsp;&emsp;回到工程，点击上方菜单栏，项目，添加引用：

![添加引用](/chapter_1/assert/添加引用.png)

&emsp;&emsp;再点击浏览，将刚才拷贝出来的文件引用。以后如果要引用更多的文件，也是这个步骤，而要引用开发包中自带的文件的话，直接在引用中的程序集一栏中查询即可。
&emsp;&emsp;本次实践需要使用以下库：System、System.Drawing、System.Windows.Forms、MikuMikuPlugin。

&emsp;&emsp;第一次实践，我们就来做一个简单的弹窗命令插件吧！
&emsp;&emsp;回到之前我们讲过的部分，命令插件包含了GUID，ApplicationForm、Scene等部分，我们需要将他们一一写出。

![命令插件接口](/chapter_1/assert/命令插件接口.png)

&emsp;&emsp;这里根据格式写即可，类名自己定义。

![GUID码](/chapter_1/assert/GUID.png)

&emsp;&emsp;GUID码，还记得吗，之前说的，这里必须和其他插件不同才行。
&emsp;&emsp;推荐一个方法，百度GUID生成器，找到一个在线生成GUID的网站，以插件名+一个随机数的格式为参数生成一个GUID。

&emsp;&emsp;其它部分按部就班即可，具体代码会在附录中给出，需要的话可以自行查阅。这里只就Run方法进行详解。
&emsp;&emsp;根据前面的介绍，Run方法内的代码就是我们按下按钮后执行的内容，因此我们需要的功能就是写在这里啦！
&emsp;&emsp;这里直接使用MessageBox类来实现，如果有自己的想法的话可以自行修改：

![Run方法](/chapter_1/assert/Run方法.png)

&emsp;&emsp;第一个参数ApplicationForm也就是命令插件接口组成中的IWin32Window，代表MMM窗口本体。这句代码的意思是以MMM窗口本体为消息窗所有者，弹出一个内容为xxx的消息弹窗。

&emsp;&emsp;写完代码之后，点击编译，生成，就可以在路径：工程目录\工程名\bin\Debug（如果选择Release模式则为Release）中找到你生成的插件啦！

![生成文件的位置](/chapter_1/assert/生成文件.png)

&emsp;&emsp;将这个dll文件拷贝到路径：MMM安装目录\Plugins中，打开MMM，就可以在插件栏中找到我们的插件了。

![插件载入完成](/chapter_1/assert/安装插件.png)

&emsp;&emsp;点击按钮，就可以看到插件弹出的消息框啦！

![弹出消息框](/chapter_1/assert/弹出消息框.png)

&emsp;&emsp;大家可以将这个工程保留，以后需要做命令插件的话就拷贝一份，这样就不用麻烦每次都把每个方法属性再写一遍啦！
&emsp;&emsp;到这里，我们已经初步认识了MMM插件，解开了它神秘的面纱，甚至自己亲自动手制作了一个。&emsp;&emsp;接下来实现其他功能，也就是构思和设计的问题了。
&emsp;&emsp;接下来几章，我会接受一些各个接口获取的数据是什么，如何构成的以及如何使用，慢慢地，大家就可以借助插件实现自己想要的功能了。
