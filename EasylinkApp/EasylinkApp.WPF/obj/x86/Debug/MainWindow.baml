   M S B A M L   `   `   `  ����  EasylinkApp.WPF     EasylinkApp.WPF.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  Employee System��5   	   $	��600��6!   $	��800��6.    � 	Resources 5       �� 
TargetType �� Property �� Value��%�   '��       '�� M     '�� �     �� 5   
   " ��6   .�5	      ��  � ��$ 0,0,0,8q�6'   5
      �� 5   
   " ��6   .�5      ��  � ��$
 Bold"�6+   5      �� 5   
   " ��6   .�5      ��  � ��$
 Bold"�6+   5      5      .��5      ��  �� Orientation$ Vertical=�6   .+�5   
   � $��5q�6    � ColumnDefinitions 5      �� 5      $��* �6$   �� 5      $��2* �6$   �� 5      $��20 �6#   �� 5      $��* �6#   �� 5      $��2* �6#   �� 5      $��20 �6#   �� 5        � RowDefinitions 5#      �� 5%      �� 5'      .+�5*      �� $��0��6   $��0��6(   First Name:6B   �� 5+      $��1��6   $��0��6'   ��65   �� *FirstNameSearch+6X   �� 5,      $��3��6   $��0��6(   
Last Name:6B   �� 5-      $��4��6   $��0��6'   ��64   �� *LastNameSearch+6V   �� 5/      $��0��6   $��1��6(   Department:6C   �� 51      $��1��6   $��1��6(    �� DisplayMemberPath$
 Name��6�   	 �� SelectedValuePath$	 Id��6�   ��65   �� *Departments+
 �� SelectedValue
 6Y   �� *DepartmentSearch+54      �� 5:      $��3��6   $��1��6(   	Active:6>   �� 5;      $��4��6   $��1��6(    �� Text 66   �� *ActiveSearch+.@�5=      �� $	��Any��6   6/   �� 5?      $	��Yes��6   6/   �� 5A      $��No��6   6.   5C      �� 5D      $��6��6   $��1��6%   $��Search��62   $	��100��6C   ��6O   ��  �� Path$ RunSearchCommand�6|   5F      �� 5H   
   $��5q�6   $��Create New Employee��6   $	��200��6=   $��Right=�6s   ��6I   �� $ RunCreateCommand�6�   &   System.Windows.Controls.DataGrid  5K   
      AutoGenerateColumns .  $��5q�60      SelectionMode$ Single=�6c   
������    
IsReadOnly . $��Normal"�6�   ��6;   �� *EmployeesFound+ �� SelectedItem 6z   �� *SelectedEmployee+   	CellStyle 5M      �� 5N      *  $System.Windows.Controls.DataGridCell"  6   .�5O      ��  � ��$ 4q�60   �� 5P       � �� 5Q      �� 5R       �� 
TargetType"  6.   .R�5S   "   ��  �� Padding# z��6)   #��z��6M   #��z��6y   #��z��6�    J� SnapsToDevicePixels . ._�5T   &   �� # z 67   5U   #   5V      5X      5Y         Columns 5]      0  *System.Windows.Controls.DataGridTextColumn  5`      ,  &System.Windows.Controls.DataGridColumn   Header$ 
First Name��6%      Width5  /System.Windows.Controls.DataGridLengthConverter$ * 69   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6D   �� *	FirstName+  5a      $ 	Last Name��6%   $ * 68    6D   �� *
LastName+  5b      $ 
Department��6%   $ * 69    6D   �� *Department.Name+  5c      $ Active��6%   $ * 65    6@   �� *Active+4  .System.Windows.Controls.DataGridTemplateColumn  5e         CellTemplate 5f      �� 5g      .R�5h      �� ��6'   ��  �� RelativeSource ��  �� AncestorType"  $  DataContext.RunEditCommand���5j   $   �� ��5k   "   ]� 5l   &   
�������.+�5m   *   �� $��\images\edit.png��61   5n   '   5p      5r      5t        5v       5w      �� 5x      .R�5y      �� ��6'   ��  �� "  $" DataContext.RunDeleteCommand���5{   $   �� ��5|   "   ]� 5}   &   
�������.+�5~   *   �� $��\images\erase.ico��61   5   '   5�      5�      5�      5�      5�      5�      