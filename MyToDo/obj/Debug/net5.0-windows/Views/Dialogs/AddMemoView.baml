   M S B A M L   `   `   `  ????A  =MyToDo, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:MyToDo.Views.DialogsMyToDo.Views.Dialogs  5      &     MyToDo.Views.Dialogs.AddMemoView   5      W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      4local"clr-namespace:MyToDo.Views.Dialogs     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      _ [MaterialDesignThemes.Wpf, Version=4.6.1.0, Culture=neutral, PublicKeyToken=df2a72020bd7962a:md1http://materialdesigninxaml.net/winfx/xaml/themes  5      $	??450??5	      $	??300??5
      .??5      ?   ? RowDefinitions  5   
   ?? 5      $
??Auto ?6   ?? 5      ?? 5      $
??Auto ?6   .+?5   
   ??  ?? Padding$ 20,10q?5      $??24(?5      $
??Bold"?5      $??添加备忘录??5      ]? 5   
   $??1??5      $
??20,0q?5      $??False??5      .+?5%      ?? $
??0,10q?5&      )  #MaterialDesignThemes.Wpf.HintAssist   Hint$ 添加备忘录概要....??5'      $	??Top=?5(      ??5)      ?? *Model.Title+?? 5*      $	??100??5+      $
??0,10q?5,      $ 添加备忘录内容....??5-      $	??Top=?5.      ??5/      ?? *Model.Content+50      ?? 51   
   $??2??52      $??10q?53      $??Right=?54       ?? Orientation$ 
Horizontal=?55      .+?56      ?? $??0,0,10,0q?57      $??取消??59       "  MaterialDesignPaperLightButton#??[  5:      ??58      ?? *CancelCommand+5:      ?? 5;      $??确定??65   ??6   ?? *SaveCommand+65   5<      5=      5>      