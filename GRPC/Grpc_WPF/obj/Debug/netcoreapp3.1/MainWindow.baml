   M S B A M L   `   `   `  ����C  ?Grpc_WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:Grpc_WPFGrpc_WPF  5   	       Grpc_WPF.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   (localclr-namespace:Grpc_WPF     5   	   -   5          � Title$5  /Könyves Bolt (Szolg.Prog. 2019/01/14 - HL5U4V)��5   	   $	��450��6@   $	��928��6M    � WindowStartupLocation$ CenterScreen=�6Y   .��5	      �  � ColumnDefinitions 5
   
   �� 5      �� 5      $	��15* �6   �� 5       � RowDefinitions 5   
   �� 5      �� 5      $	��10* �6   �� 5      $��2* �6   �� 5      $��1* �6   .+�5   
   &   System.Windows.Controls.DataGrid  -   5          Name$ datagrid_main��5   0   $��1��6   $��1��6       SelectionMode$ Single=�6e      
IsReadOnly$
 True��6|   5"      �� 5#   
    �� Orientation$ 
Horizontal=�6   $��2��6.   $��1��6;   $��Center=�6K   $��Center=�6h   .+�5$      �� -   5         ��Name$ LoginButton��5$      $��Bejelentkezés��6*   $
��10pxq�6B   $
��10pxq�6P   6_   �� 5%      -   5        $ 	NewButton��5%      $	��Új��6(   $
��10pxq�65   $
��10pxq�6C   $��60��6R   6`   �� 5&      -   5        $ DeleteButton��5&      $��Törlés��6+   $
��10pxq�6<   $
��10pxq�6J   6Y   �� 5'      -   5        $ 
EditButton��5'      $��Szerkesztés��6)   $
��10pxq�6?   $
��10pxq�6M   6\   �� 5(      -   5        $ 	BuyButton��5(      $��Vásárlás��6(   $
��10pxq�6;   $
��10pxq�6I   6X   �� 5)      -   5        	 ��Name$	 TextBox_BookTitle��5)      $��230px��61   $
��10pxq�6B   $��Stretch=�6|   5,      5-      5.      