   M S B A M L   `   `   `  ����C  ?Grpc_WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:Grpc_WPFGrpc_WPF  5   	       Grpc_WPF.BookDataWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   (localclr-namespace:Grpc_WPF     5   	   -   5          � Title$  BookDataWindow��5   	   $	��517��6    $	��677��6-   .��5	      �  � ColumnDefinitions 5
   
   �� 5      $��3* �6   �� 5      $��2* �6   .+�5   
   ��  �� CanVerticallyScroll$
 True��6   .+�5      ��  �� Orientation$ 
Horizontal=�6   $��Center=�62   $	��5pxq�6O   .+�5      �� $	��ID:��6   $	��5pxq�6&   $��50��63   �� 5      -   5         ��Name$ inp_ID��5      $	��200��6*   $	��5pxq�69    �� 
IsReadOnly$
 True��6F   5      �� 5      $ 
Horizontal=�6   $��Center=�62   $	��5pxq�6O   .+�5      �� $��Cím:��6   $	��5pxq�6'   $��50��64   �� 5      -   5        $ 	inp_Title��5      $	��200��6-   $	��5pxq�6<   5      �� 5      $ 
Horizontal=�6   $��Center=�62   $	��5pxq�6O   .+�5      �� $��Szerző:��6   $	��5pxq�6*   $��50��67   �� 5      -   5        $ 
inp_Author��5      $	��200��6.   $	��5pxq�6=   5      �� 5      $ 
Horizontal=�6   $��Center=�62   $	��5pxq�6O   .+�5      �� $��Kiadó:��6   $	��5pxq�6)   $��50��66   �� 5      -   5        $ inp_Publisher��5      $	��200��61   $	��5pxq�6@   5      �� 5      $ 
Horizontal=�6   $��Center=�62   $	��5pxq�6O   .+�5       �� $��ISBN:��6   $	��5pxq�6(   $��50��65   �� 5!      -   5        $ inp_ISBN��5!      $	��200��6,   $	��5pxq�6;   5"      �� 5#      $ 
Horizontal=�6   $��Center=�62   $	��5pxq�6O   .+�5$      �� $��Műfaj:��6   $	��5pxq�6)   $��50��66   �� 5%      -   5        $ 	inp_Genre��5%      $	��200��6-   $	��5pxq�6<   5&      �� 5'      $ 
Horizontal=�6   $��Center=�62   $	��5pxq�6O   .+�5(      �� $
��Év:��6   $	��5pxq�6&   $��50��63   �� 5)      -   5        $ inp_PublishYear��5)      $	��200��63   $	��5pxq�6B    �� 	MaxLength$ 4��6�   5*      �� 5+      $ 
Horizontal=�6   $��Center=�62   $	��5pxq�6O   .+�5,      �� $
��Ár:��6   $	��5pxq�6&   $��50��63   �� 5-      -	   5        $ 	inp_Price��5-      $	��200��6-   $	��5pxq�6<   6I   5.      �� 5/      $ 
Horizontal=�6   $��Center=�62   $	��5pxq�6O   .+�50      �� $��Készleten:��6   $	��5pxq�6-   $��50��6:   �� 51      -
   5        $ inp_OnStorage��51      $	��200��61   $	��5pxq�6@   6M   52      57      �� 58   
   $��1��6   .+�59      �� $��	Leírás:��6   �� 5:      -   5        $ inp_Description��5:      $��200px��6/   $��200px��6@    �� AcceptsReturn$
 True��6R   �� 5;      $ 
Horizontal=�6   $	��5pxq�62   $��Center=�6?   .+�5<      �� -   5         ��Name$ OkButton��5<      $��OK��6B   $
��10pxq�6O   $	��5pxq�6]   �� 5=      -   5        $ CancelButton��5=      $��Mégsem��6J   $
��10pxq�6[   $	��5pxq�6i   �� 5>      -   5        $ ResetButton��5>      $��Alaphelyzetbe��6H   $
��10pxq�6`   $	��5pxq�6n   5?      5@      5A      5B      