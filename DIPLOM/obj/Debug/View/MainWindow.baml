   M S B A M L   `   `   `  ����A  =DIPLOM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:DIPLOM.ViewDIPLOM.View  5   	   O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089I2clr-namespace:System.Collections;assembly=mscorlibSystem.Collections 5   	   +clr-namespace:DIPLOM.ModelDIPLOM.Model  5   	   )clr-namespace:DIPLOM.ViewDIPLOM.View  5
   	   +clr-namespace:DIPLOM.ModelDIPLOM.Model  5   	       DIPLOM.View.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   >d2http://schemas.microsoft.com/expression/blend/2008    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   +localclr-namespace:DIPLOM.View     5   	   Bcol2clr-namespace:System.Collections;assembly=mscorlib     5   	   -local1clr-namespace:DIPLOM.Model     5   	   'vclr-namespace:DIPLOM.View     5
   	   )vmclr-namespace:DIPLOM.Model     5   	     � Title$  Главное окно��5   	   $	��482��6   $
��1009��6+    � DataContext 5      $   DIPLOM.View.AutorizationWindow  5   
   .��5      �  � RowDefinitions 5   
   �� 5      $��20 �6   6*   �� 5      $��* �6   6)   �� 5      $��20 �6   6*   .+�5   
   ]� $��0��6   $��0��6$   .+�5      o� $	��Top=�6   .@�5      m� $��Справочники��6   .@�5      m� -   5        $)��#Авто-комплектующие��5      6;   m� 5      $��Комплектации��6   m� 5      $%��Группы запчастей��6   5      5      5      ]� 5    
   $��0��6   $��1��6$   ��. $��
10,10,10,0q�6F   $	��250��6Z   $
��Auto��6j   .+�5!      ]� $
��Left=�6   ��. $	��200��6C   $��	10,10,0,0q�6R   .+�5"      �� -   5         ��Name$ 
label_Copy��5"   -   $	��Top=�6   $I��CВыберите необходимую совместимость:��6A   $��29��6o   �� 5#      -   5        $��ComboBoxCompatibility��5#   0   $	��Top=�6    �� DisplayMemberPath$
 Name��6M   $��25��6g   6v   &   System.Windows.Controls.DataGrid  5$      -   5          Name$ RequestedGroups��5$         AutoGenerateColumns .  $	��Top=�5%      $��	0,10,0,10q�60   $��50��5&         CanUserResizeRows .     CanUserAddRows .  $	��130��5'   2   	   Columns	 5(      0  *System.Windows.Controls.DataGridTextColumn  5)      ,  &System.Windows.Controls.DataGridColumn
   Header$
 Группа��6-      Width5  /System.Windows.Controls.DataGridLengthConverter$ * 6W      MinWidth$	 100Q�6a      MaxWidth$	 170Q�6p      
IsReadOnly . 1  +System.Windows.Controls.DataGridBoundColumn   Binding 6=   �� *Group+6   4  .System.Windows.Controls.DataGridCheckBoxColumn  5*      $
 Выбрана��61   $ 60 6a    6B   �� *
_isChecked+5+      5-        5.      -   5        $ RequestedParts��5.       .  $	��Top=�5/      $��0,0,0,10q�60   $��50��50       .   .  $	��150��51   2   	 52        53      $
 Запчасть��6-   $ * 6\   $	 100Q�6f   $	 170Q�6u    .  6?   �� *
AutoPart+6�     54      $
 Выбрана��61   $ 60 6a    6B   �� *
_isChecked+55      57      58      �� 59      $
��Left=�6   $	��200��6/   $��	10,10,0,0q�6>   .+�5:      �� -   5        $ label��5:      $5��/Введите ценовой параметр:��6'   $��29��6K   $	��Top=�6W   �� 5;      -   5         ��Name$ textBoxPrice��5;      $��23��60    �� TextWrapping$
 Wrap=�6<   $�� ��6P   $	��Top=�6Y   �� 5<      -   5        $ label_Copy2��5<      $1��+Введите параметр массы:��6-    J� RenderTransformOrigin$ 0.596,0.517&�6O   $��29��6s   $	��Top=�6   �� 5=      -	   5        $ textBoxWeight��5=      $��23��61   $
 Wrap=�6=   $�� ��6Q   $	��Top=�6Z   �� 5>      -
   5        $ label_Copy3��5>      $3��-Введите параметр объема:��6-   $ 0.596,0.517&�6P   $��29��6t   $	��Top=�6�   �� 5?      -   5        $ textBoxCapacity��5?      $��23��63   $
 Wrap=�6?   $�� ��6S   $	��Top=�6[   �� 5@      -   5         ��Name$ buttonStart��5@      $+��%Отобрать компоненты��6.   $��25��6M   $��0,10,0,0q�6Y   $	��Top=�6k   6�   �� 5A      -   5        $ buttonCancel��5A      $��Отмена��6/   $��25��6@   $��0,10,0,0q�6L   {�.  6q   �� 5B      -   5        $ buttonOK��5B      $-��'Зафиксировать подбор��6+   $��25��6J   $��0,10,0,0q�6V   {�.  6{   �� 5C      -   5        $ buttonSaveReport��5C      $#��Сохранить отчет��63   $��25��6M   $��0,10,0,0q�6Y   {�.  6~   5E      ]� 5F      ��. $��
10,10,10,0q�6-   $	��250��6A   .+�5G      �� -   5        $ label_Copy1��5G      $	��Top=�6-   $(��"Результат подбора:��6B   $��29��6_     5H      -   5        $ dataGridParts��5H         
IsReadOnly .  .  $��0,0,0,10q�6`   	 5I        5J      $
 Id��6-   $ 50Q�6P    69   �� *Id+6P     5K      $
 Название��6-   $ * 6X   $	 100Q�6b    6?   �� *Name+6b     5L      $
 Группа��6-   $	 100Q�6[    6=   �� *	GroupName+6[     5M      $
 Цена��6-   $	 100Q�6U    6;   �� *Price+6U     5N      $
 Вес��6-   $	 100Q�6U    6:   �� *Weight+6U     5O      $
 Размеры��6-   $	 100Q�6^    6>   �� *Proportions+6^   5Q      5R      5S      � 5T   
   -   5         �Name$ progressBar��5T      $��0��6+   $��2��6;    � IsIndeterminate . $��0,10,0,0q�6_   $y�Hidden=�6q   5U      5V      