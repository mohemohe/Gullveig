Gullveig
====

わずか16行のXAMLでナウい感じになるオレオレウィンドウフレームワーク  
タスクトレイアイコン・メニューにも対応

※ 平成28年度　北海道科学大学　卒業研究の成果物の副産物です

[![](http://i.imgur.com/hT1IK6q.png)](http://i.imgur.com/hT1IK6q.png)

``` xaml
<g:Window x:Name="Window" x:Class="Gullveig.Test.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:g="clr-namespace:Gullveig;assembly=Gullveig"
        Title="Gullveig.Test" Height="350" Width="525" RegisterTaskTrayIcon="True" Icon="test.png">
    <g:BorderedWindow AccentColor="#FFE60E0E">
        <g:CaptionBar>
            <TextBlock Text="{Binding Title, ElementName=Window}" VerticalAlignment="Center" HorizontalAlignment="Center" Foreground="White" Margin="4,0,0,0" FontFamily="Meiryo UI" FontSize="14.667"/>
            <g:CaptionButtons HorizontalAlignment="Right" />
        </g:CaptionBar>
        <g:ContentArea>
            <Label>test!</Label>
        </g:ContentArea>
        <g:StatusBar Name="StatusBar" Text="ババーン"/>
    </g:BorderedWindow>
</g:Window>
```