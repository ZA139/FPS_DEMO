![](https://img.shields.io/badge/license-GPL3.0-green.svg)
# FPS_DEMO
V0.1键位(2019-06-19)

	WASD，空格----------------------------------------------------移动跳跃
	T键---------------------------------------------------切换特殊武器弹夹
	1----------------------------------------------------------压入麻痹弹药
	2----------------------------------------------------------压入火炎弹药
	3------------------------------------------------------------压入达姆弹
	4----------------------------------------------------------压入电击弹药
	5----------------------------------------------------------压入普通弹药
	K键--------------------------------------------------------打开技能菜单
	开启技能菜单后
	1-------------------------------------------------------升级攻击力（喝大力）
	2----------------------------------------------------升级速度（香港记者变身）
	3-----------------------------------------------------升级生命值（同时回满血）
电击没做，对应特效没有。
V0.2键位(2019-06-22)

```
T键---------------------------------------------------切换特殊武器弹夹
1----------------------------------------------------------压入麻痹弹药
2----------------------------------------------------------压入火炎弹药
3------------------------------------------------------------压入达姆弹
4----------------------------------------------------------压入电击弹药
5----------------------------------------------------------压入普通弹药
K键--------------------------------------------------------打开技能菜单
ESC------------------------------------------------------------退出跑路
开启技能菜单后
1-------------------------------------------------------升级攻击力（喝大力）
2----------------------------------------------------升级速度（香港记者变身）
3-----------------------------------------------------升级生命值（同时回满血）
```

电击没做，对应特效没有，特殊弹药获取没做。
V0.2主要更新：

添加了Badass BOSS的模型和动画切换，特殊弹药对于BadAss的效果有某个方面的提升，修改了游戏平衡。

V0.3更新说明:  
修复了普通僵尸对于特殊弹药无伤害的BUG.  
未修复BUG：有时瞄准会出现射线判定的BUG  
（详述：因为特殊弹药造成的动画需要在另外一个layer层进行判断，所以需要用另一组射线来保存在判断动画的Badass Layer层的碰撞信息，因为模型碰撞体使用的是柱型碰撞体(Capsule Collider)而不是面碰撞体（Mesh Collider），所以若击中两者的重合点在碰撞判断上可能会出现击中判断在zombie而动画判断在Badass这样的情况，因为这是属于美术模型问题，具体细节需要耗费较多时间，故本次课程设计版本暂不修复此BUG）

V0.4更新说明  
修复了V0.3未修复的BUG  
课程设计最终版本  



本课程设计使用的部分美术资源分别来自于:
金玺曾.Unity 3D/2D手机游戏开发：从学习到产品[M].北京：清华大学出版社，2017.
敌人处于燃烧状态Flame.ogg音效 
作者：themightyglider
来源：https://opengameart.org/content/catching-fire 
CC协议版本0.0（PUBLIC DOMAIN）
BadAss模型及动画
作者：Rosswet Mobile
来源：https://opengameart.org/content/thin-zombie-awake-zombie-asset
CC协议版本3.0（by）
特殊弹药Shot音效
	作者：Michel Baradari
	来源：https://opengameart.org/content/chaingun-pistol-rifle-shotgun-shots
CC协议版本3.0（by）
特别感谢沈阳建筑大学通信工程大一温兆熙制作的背景音乐
BadAss出场音乐
Divide-------------Magna----《Hotline Miami 2 Wrong Number Soundtrack》
