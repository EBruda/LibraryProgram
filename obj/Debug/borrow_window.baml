   M S B A M L   `   `   `  ����B  >Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:LibraryLibrary  5   	   O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem 5   	       Library.borrow_window   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:Library     5   	   6sys&clr-namespace:System;assembly=mscorlib     5   	     � Title$  Borrow a Book��5	   	   $��498.333��6   $	��601��60    � 
ResizeMode$ NoResize=�6=   .��5
      � $	��505��6   $	��Top=�6   .+�5   
   �� -   5         ��Name$ names_combo��5      $
��Left=�6(   $��243,315,0,0q�6C   $	��Top=�6X   $	��120��6p    �� 
IsEditable . ��6|   ��  � 	Resources 5      ��%X   (��       5      *,*SystemParameters.VerticalScrollBarWidthKey+)X�6   0Q�6^   5      5      �� 5   
   -   5         ��Name$ submit_borrow_btn��5      $��Borrow��6,   $��243,397,230,0q�6=   $	��Top=�6T   $��30��6�   �� 5   
   $��Borrowed by:��6   $
��Left=�6'   $	��Top=�6B   $��148,311,0,0q�6Z   �� 5   
   $��Author:��6   $
��Left=�6"   $��179,254,0,0q�6=   $	��Top=�6R   �� 5   
   $��Title:��6   $
��Left=�6!   $��193,210,0,0q�6<   $	��Top=�6Q   �� 5   
   $	��Id:��6   $
��Left=�6   $��
205,82,0,0q�69   $	��Top=�6M   �� 5   
   -   5         ��Name$ show_id��5      $�� ��6!   $
��Left=�6,   $��
243,82,0,0q�6G   $	��Top=�6[   �� 5   
   -   5        $ 
show_title��5      $�� ��6$   $
��Left=�6/   $��243,210,0,0q�6J   $	��Top=�6_   �� 5   
   -   5        $ show_author��5      $�� ��6%   $
��Left=�60   $��243,254,0,0q�6K   $	��Top=�6`   �� 5   
   $��Borrow a book��6   $
��Left=�6(   $��
223,22,0,0q�6C   $	��Top=�6W   $��20(�6o   �� 5   
   $��ISBN:��6   $
��Left=�6    $��190,125,0,0q�6;   $	��Top=�6P    J� RenderTransformOrigin$ -1.36,0.731&�6h   �� 5   
   $��
GRBC Code:��6   $
��Left=�6%   $��155,170,0,0q�6@   $	��Top=�6U   �� 5   
   -   5        $ 	show_isbn��5      $�� ��6$   $
��Left=�6/   $��243,125,0,0q�6J   $	��Top=�6_   �� 5   
   -   5        $ show_grbc_code��5      $�� ��6(   $
��Left=�63   $��243,170,0,0q�6N   $	��Top=�6c   $ 0.133,0.449&�6{   5      5      