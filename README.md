
## 主要功能


主要用于游戏中的HUD合批，实现了一个批次渲染所有HUD


## 核心技术

1.使用了Text渲染到纹理Array，参考


## 效果演示
<br><img src='image/1.png'><br>



## 其他注意

1.一次可以提交500个HUD，如果超过500，需要构建多个MaterialPropertyBlock，然后多次调用Graphics.DrawMeshInstanced
