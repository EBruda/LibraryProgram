   M S B A M L   `   `   `  ����B  >Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:LibraryLibrary  5   	       Library.edit_window   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:Library     5   	   -   5          � Title$  Edit an Existing Item��5   	   $��523.001��6'   $	��535��68    � 
ResizeMode$ NoResize=�6D   .��5	      � $��0,0,-6,1q�6   .+�5   
   �� $��Editing an Existing Item ��6   $
��Left=�64   $	��Top=�6O   $��20(�6g   $��
141,30,0,0q�6u   $��37��6�   $	��222��6�   �� 5   
   $��        Title:��6   $
��Left=�6)   $��
99,326,0,0q�6D   $	��Top=�6X   $��26��6p   $��61��6|   �� 5   
   $��        Author:��6   $
��Left=�6*   $��
85,366,0,0q�6E   $	��Top=�6Y   $��26��6q   $��75��6}   �� 5   
   $��        ID:��6   $
��Left=�6&   $��112,214,0,0q�6A   $	��Top=�6V   $��26��6n   $��50��6z   �� 5   
   -   5         ��Name$ 
new_author��5      $
��Left=�6&   $��23��6A    �� TextWrapping$
 Wrap=�6M   $	��Top=�6a   $	��202��6y   $��180,369,0,0q�6�   �� 5   
   -   5        $ 	new_title��5      $
��Left=�6%   $��23��6@   $
 Wrap=�6L   $	��Top=�6`   $	��202��6x   $��180,328,0,0q�6�   �� 5   
   -   5         ��Name$ 
cancel_btn��5      $��Cancel��6%   $
��Left=�66   $��75��6Q   $��143,440,0,0q�6\   $��22��6�   $	��Top=�6�    J� RenderTransformOrigin$ 0.293,-2.909&�6�   �� 5   
   -   5        $ 
submit_btn��5      $��Submit��6%   $
��Left=�66   $	��Top=�6Q   $��75��6i   $��288,440,0,0q�6t   $��22��6�   �� 5   
   $|��vNote: If there are no changes then leave it as it is. 
You can not change the id since it is used to identify the book��6   $
��Left=�6�   $	��Top=�6�   $��	99,81,0,0q�6�   $��42��6�   $	��379��6�   �� 5   
   -   5         ��Name$ id_label��5      $�� ��6"   $
��Left=�6-   $��182,214,0,0q�6H   $	��Top=�6]   $��36��6u   $��26��6�   �� 5   
   $��Holder:��6   $
��Left=�6"   $��113,146,0,0q�6=   $	��Top=�6R   $ -0.388,0.231&�6j   �� 5   
   -   5        $ 
new_holder��5      $�� ��6$   $
��Left=�6/   $	��Top=�6J   $��182,146,0,0q�6b   �� 5   
   $��Status:��6   $
��Left=�6"   $��117,177,0,0q�6=   $	��Top=�6R   �� 5   
   -   5        $ 
new_status��5      $�� ��6$   $
��Left=�6/   $��182,177,0,0q�6J   $	��Top=�6_   �� 5   
   $��ISBN:��6   $
��Left=�6    $��124,251,0,0q�6;   $	��Top=�6P   �� 5   
   $��
GRBC Code:��6   $
��Left=�6%   $��
89,288,0,0q�6@   $	��Top=�6T   �� 5   
   -	   5        $ new_isbn��5      $
��Left=�6$   $��23��6?   $��180,254,0,0q�6K   $
 Wrap=�6`   $	��Top=�6t   $	��202��6�   �� 5   
   -
   5        $ new_grbc_code��5      $
��Left=�6)   $��23��6D   $��180,291,0,0q�6P   $
 Wrap=�6e   $	��Top=�6y   $	��202��6�   5      5      