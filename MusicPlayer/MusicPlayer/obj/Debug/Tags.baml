   M S B A M L   `   `   `  ����F  BMusicPlayer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:MusicPlayerMusicPlayer  5   	       MusicPlayer.Tags   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   +localclr-namespace:MusicPlayer     5   	     � Title$
  Tags��5   	   $	��350��6   $	��300��6#    � 
ResizeMode$ NoResize=�6/   $y�Visible=�6E    � WindowStartupLocation$ CenterScreen=�6Z    � WindowStyle$
 None=�6    � 	Resources 5
       �� Source�� 5   
   $ Dictionary_en.xaml?�6   %    6;   .��5      � .+�5   
   �� ���#C4C4C4���#616161$��1q�6<   ._�5      � -   5        $��TitleBar��5   Q   $��20��6   $	��Top=�6   ���#9D9D9D.+�5      �� -   5         ��Name$ ExitTags��5      $��20��6,   $��20��67   ���#616161$��Right=�6k   $��Center=�6�   .��5      �� $/��)pack://application:,,,/Resource/close.png��6   $��15��6O   $��15��6Z   $��
-13,-2,0,0q�6f   5      5      5      � 5   
    � ColumnDefinitions 5      �� 5      6%   �� 5      6%   �� 5      6%    � RowDefinitions 5      �� 5      6"   �� 5      6"   �� 5      6"   �� 5       6"   �� 5!      6"   �� 5"      6"   �� 5#      6"   �� 5$      6"   �� 5%      6"   .+�5'      ��    
txt_Number#���   6   $��0��6<   $��1��6L   $
��Left=�6Z   	 �� Padding$		 5,0q�6u   $��16(�6�   
���   �6�   �� 5(        txt_Song#���  6   $��0��6:   $��2��6J   $
��Left=�6X   $		 5,0q�6s   $��16(�6�   
���   �6�   �� 5)        	txt_Album#���  6   $��0��6;   $��3��6K   $
��Left=�6Z   $		 5,0q�6u   $��16(�6�   
���   �6�   �� 5*        txt_Band#���  6   $��0��6:   $��4��6J   $
��Left=�6Y   $		 5,0q�6t   $��16(�6�   
���   �6�   �� 5+        txt_Year#���  6   $��0��6:   $��5��6J   $
��Left=�6Y   $		 5,0q�6t   $��16(�6�   
���   �6�   �� 5,        txt_Bitrate#���  6   $��0��6=   $��6��6M   $
��Left=�6\   $		 5,0q�6w   $��16(�6�   
���   �6�   �� 5-      $��
Other info��6   $��0��6*   $��7��6:   $
��Left=�6I   $		 5,0q�6d   $��16(�6r   
���   �6�   �� 5/      -   5        
 ��Name$
 TBNumber��5/      $��1��6(   $��2��68   $��1��6L   $	��150��6Y   $��20��6e   $	��0,0q�6q   $��	0,-10,0,0q�6    �� 	MaxLength$ 3��6�   �� 50      -   5        $
 TBSong��50      $��1��6&   $��2��66   $��2��6J   $	��150��6W   $��20��6c   $	��0,0q�6o   $��	0,-10,0,0q�6}   $ 55��6�   �� 51      -   5        $
 TBAlbum��51      $��1��6'   $��2��67   $��3��6K   $	��150��6X   $��20��6d   $	��0,0q�6p   $��	0,-10,0,0q�6~   $ 55��6�   �� 52      -   5        $
 TBBand��52      $��1��6&   $��2��66   $��4��6J   $	��150��6W   $��20��6c   $	��0,0q�6o   $��	0,-10,0,0q�6}   $ 55                      ��6�   �� 54      -   5        $
 TBYear��54      $��1��6&   $��2��66   $��5��6J   $	��150��6W   $��20��6c   $	��0,0q�6o   $��	0,-10,0,0q�6}   $ 4��6�   �� 55      -   5        $
 	TBBitrate��55      $��1��6)   $��2��69   $��6��6M   $	��150��6Z   $��20��6f   $	��0,0q�6r   $��	0,-10,0,0q�6�    �� 
IsReadOnly . 6�   �� 56      -	   5        $
 
TBChannels��56      $��1��6*   $��2��6:   $��7��6N   $	��150��6[   $��20��6g   $	��0,0q�6s   $��	0,-10,0,0q�6�    . 6�   �� 58      -
   5        $ SaveTags��58   ;     txt_Save#���  6   $��0��6M   $��8��6]   $��30��6j   $��Center=�6v   6�   �� 59      -   5        $ 	CloseTags��59   9    
 txt_Ex#���  6   $��2��6L   $��8��6\   $��30��6i   $��Center=�6u   6�   5:      5;      5<      