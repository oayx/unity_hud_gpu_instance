
## 主要功能


主要用于游戏中的HUD合批，实现了一个批次渲染所有HUD


## 技术介绍

1.使用了Text渲染到纹理Array，参考FontRender2Texture

2.构建Instance需要的Mesh，参考HUDMeshBuild

3.HUDInstanced使用Graphics.DrawMeshInstanced绘制


## 效果演示
<br><img src='image/1.png'><br>


## 与普通UGUI方式比较(渲染1000个HUD，进度条每帧更新)

1.以下是直接使用UGUI渲染，一个Image+一个Text
<br><img src='image/4.png'><br>
<br><img src='image/5.png'><br>
2.以下是使用GPU Instance的版本
<br><img src='image/3.png'><br>
<br><img src='image/2.png'><br>
3.使用GPU Instance，渲染1000个HUD，基本上可以在2DrawCall和1MS CPU开销完成


## 其他注意

1.一次可以提交500个HUD，如果超过500，需要构建多个MaterialPropertyBlock，然后多次调用Graphics.DrawMeshInstanced
